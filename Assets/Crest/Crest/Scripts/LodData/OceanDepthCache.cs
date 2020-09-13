// Crest Ocean System for URP

// Copyright 2019 Huw Bowles

using UnityEngine;
using UnityEngine.Rendering;

namespace Crest
{
    /// <summary>
    /// Renders terrain height / ocean depth once into a render target to cache this off and avoid rendering it every frame.
    /// This should be used for static geometry, dynamic objects should be tagged with the Render Ocean Depth component.
    /// </summary>
    public class OceanDepthCache : MonoBehaviour, IValidated
    {
        [Tooltip("The layers to render into the depth cache."), SerializeField]
        string[] _layerNames = null;

        [Tooltip("The resolution of the cached depth - lower will be more efficient."), SerializeField]
        int _resolution = 512;

        // A big hill will still want to write its height into the depth texture
        [Tooltip("The 'near plane' for the depth cache camera (top down)."), SerializeField]
        float _cameraMaxTerrainHeight = 100f;

        [Tooltip("Will render into the cache every frame. Intended for debugging, will generate garbage."), SerializeField]
#pragma warning disable 414
        bool _forceAlwaysUpdateDebug = false;
#pragma warning restore 414

        RenderTexture _cacheTexture;
        GameObject _drawCacheQuad;
        Camera _camDepthCache;

        void Start()
        {
            if (_layerNames == null || _layerNames.Length < 1)
            {
                Debug.LogError("At least one layer name to render into the cache must be provided.", this);
                enabled = false;
                return;
            }

            if (OceanRenderer.Instance == null)
            {
                enabled = false;
                return;
            }

            if (transform.lossyScale.magnitude < 5f)
            {
                Debug.LogWarning("Ocean depth cache transform scale is small and will capture a small area of the world. Is this intended?", this);
            }

            if (_forceAlwaysUpdateDebug)
            {
                Debug.LogWarning("Note: Force Always Update Debug option is enabled on depth cache " + gameObject.name, this);
            }

        }

        private void OnEnable()
        {
            RenderPipelineManager.beginCameraRendering += BeginCameraRendering;
        }
        private void OnDisable()
        {
            RenderPipelineManager.beginCameraRendering -= BeginCameraRendering;
        }

        RenderTexture MakeRT(bool depthStencilTarget)
        {
            var fmt = depthStencilTarget ? RenderTextureFormat.Depth : RenderTextureFormat.RHalf;
            Debug.Assert(SystemInfo.SupportsRenderTextureFormat(fmt), "The graphics device does not support the render texture format " + fmt.ToString());
            var result = new RenderTexture(_resolution, _resolution, depthStencilTarget ? 24 : 0);
            result.name = gameObject.name + "_oceanDepth_" + (depthStencilTarget ? "DepthOnly" : "Cache");
            result.format = fmt;
            result.useMipMap = false;
            result.anisoLevel = 0;
            return result;
        }

        bool InitObjects()
        {
            if (_cacheTexture == null)
            {
                _cacheTexture = MakeRT(false);
            }

            if (_drawCacheQuad == null)
            {
                _drawCacheQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                _drawCacheQuad.hideFlags = HideFlags.DontSave;
                Destroy(_drawCacheQuad.GetComponent<Collider>());
                _drawCacheQuad.name = "Draw_" + _cacheTexture.name + "_NOSAVE";
                _drawCacheQuad.transform.SetParent(transform, false);
                _drawCacheQuad.transform.localEulerAngles = 90f * Vector3.right;
                _drawCacheQuad.AddComponent<RegisterSeaFloorDepthInput>();
                var qr = _drawCacheQuad.GetComponent<Renderer>();
                qr.material = new Material(Shader.Find(LodDataMgrSeaFloorDepth.ShaderName));
                qr.material.mainTexture = _cacheTexture;
                qr.enabled = false;
            }

            if (_camDepthCache == null)
            {
                var errorShown = false;
                var layerMask = 0;
                foreach (var layer in _layerNames)
                {
                    int layerIdx = LayerMask.NameToLayer(layer);
                    if (string.IsNullOrEmpty(layer) || layerIdx == -1)
                    {
                        Debug.LogError("OceanDepthCache: Invalid layer specified: \"" + layer +
                        "\". Does this layer need to be added to the project (Edit/Project Settings/Tags and Layers)? Click this message to highlight the cache in question.", this);
                        errorShown = true;
                    }
                    else
                    {
                        layerMask = layerMask | (1 << layerIdx);
                    }
                }

                if (layerMask == 0)
                {
                    if (!errorShown)
                    {
                        Debug.LogError("No valid layers for populating depth cache, aborting.", this);
                    }

                    return false;
                }

                _camDepthCache = new GameObject("DepthCacheCam").AddComponent<Camera>();
                _camDepthCache.transform.position = transform.position + Vector3.up * _cameraMaxTerrainHeight;
                _camDepthCache.transform.parent = transform;
                _camDepthCache.transform.localEulerAngles = 90f * Vector3.right;
                _camDepthCache.orthographic = true;
                _camDepthCache.orthographicSize = Mathf.Max(transform.lossyScale.x / 2f, transform.lossyScale.z / 2f);
                _camDepthCache.cullingMask = layerMask;
                _camDepthCache.clearFlags = CameraClearFlags.SolidColor;
                // Clear to 'very deep'
                _camDepthCache.backgroundColor = Color.white * 1000f;
                _camDepthCache.enabled = false;
                _camDepthCache.allowMSAA = false;
                _camDepthCache.gameObject.SetActive(false);
            }

            if (_camDepthCache.targetTexture == null)
            {
                _camDepthCache.targetTexture = MakeRT(true);
            }

            return true;
        }

        public void BeginCameraRendering(ScriptableRenderContext context, Camera camera)
        {
            // Make sure we have required objects
            if (!InitObjects())
            {
                enabled = false;
                RenderPipelineManager.beginCameraRendering -= BeginCameraRendering;
                return;
            }

            // Render scene, saving depths in depth buffer
            UnityEngine.Rendering.Universal.UniversalRenderPipeline.RenderSingleCamera(context, _camDepthCache);

            Material copyDepthMaterial = new Material(Shader.Find("Crest/Copy Depth Buffer Into Cache"));

            copyDepthMaterial.SetTexture("_CamDepthBuffer", _camDepthCache.targetTexture);

            // Zbuffer params
            //float4 _ZBufferParams;            // x: 1-far/near,     y: far/near, z: x/far,     w: y/far
            float near = _camDepthCache.nearClipPlane, far = _camDepthCache.farClipPlane;
            copyDepthMaterial.SetVector("_CustomZBufferParams", new Vector4(1f - far / near, far / near, (1f - far / near) / far, (far / near) / far));

            // Altitudes for near and far planes
            float ymax = _camDepthCache.transform.position.y - near;
            float ymin = ymax - far;
            copyDepthMaterial.SetVector("_HeightNearHeightFar", new Vector2(ymax, ymin));

            // Copy from depth buffer into the cache
            Graphics.Blit(null, _cacheTexture, copyDepthMaterial);

            if (!_forceAlwaysUpdateDebug)
            {
                _camDepthCache.targetTexture = null;
                RenderPipelineManager.beginCameraRendering -= BeginCameraRendering;
            }
        }

#if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(1f, 0f, 1f));
            Gizmos.color = new Color(1f, 1f, 1f, 0.2f);
            Gizmos.DrawCube(Vector3.up * _cameraMaxTerrainHeight / transform.lossyScale.y, new Vector3(1f, 0f, 1f));
        }
#endif

        public bool Validate(OceanRenderer ocean)
        {
            if (_layerNames == null || _layerNames.Length == 0)
            {
                Debug.LogError("Validation: No layers specified for rendering into depth cache, and no geometries manually provided. Click this message to highlight the cache in question.", this);
            }

            if (transform.lossyScale.magnitude < 5f)
            {
                Debug.LogWarning("Validation: Ocean depth cache transform scale is small and will capture a small area of the world. The scale sets the size of the area that will be cached, and this cache is set to render a very small area. Click this message to highlight the cache in question.", this);
            }

            if (_forceAlwaysUpdateDebug)
            {
                Debug.LogWarning("Validation: Force Always Update Debug option is enabled on depth cache " + gameObject.name + ", which means it will render every frame instead of running from the cache. Click this message to highlight the cache in question.", this);
            }

            if (Mathf.Abs(transform.position.y - ocean.transform.position.y) > 0.00001f)
            {
                Debug.LogWarning("Validation: It is recommended that the cache is placed at the same height (y component of position) as the ocean, i.e. at the sea level. If the cache is created before the ocean is present, the cache height will inform the sea level. Click this message to highlight the cache in question.", this);
            }

            var rend = GetComponentInChildren<Renderer>();
            if (rend != null)
            {
                Debug.LogWarning("Validation: It is not expected that a depth cache object has a renderer component in its hierarchy. The cache is typically attached to an empty GameObject. Please refer to the example content.", rend);
            }

            foreach (var layerName in _layerNames)
            {
                var layer = LayerMask.NameToLayer(layerName);
                if (layer == -1)
                {
                    Debug.LogError("Invalid layer specified for objects/geometry providing the ocean depth: \"" + layerName +
                        "\". Does this layer need to be added to the project (Edit/Project Settings/Tags and Layers)? Click this message to highlight the cache in question.", this);
                }
            }

            // We used to test if nothing is present that would render into the cache, but these could probably come from other scenes, and AssignLayer means
            // objects can be tagged up at run-time.

            if (_resolution < 4)
            {
                Debug.LogError("Cache resolution " + _resolution + " is very low. Is this intentional? Click this message to highlight the cache in question.", this);
            }

            return true;
        }
    }
}
