using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GetWebTexture : MonoBehaviour
{
    

    void Start()
    {
        StartCoroutine(GetTexture());
    }

    IEnumerator GetTexture()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://www.linuslepschies.de/GalleriePictures/Uploads/" + 1 + ".png");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            this.GetComponent<MeshRenderer>().materials[1].mainTexture = myTexture;
        }
    }
}
