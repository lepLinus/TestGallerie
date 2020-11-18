using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GetWebTexture : MonoBehaviour
{
    public Texture2D Webtext;
    
    void Start()
    {
        StartCoroutine(GetTexture());
    }

    IEnumerator GetTexture()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://www.linuslepschies.de/GalleriePictures/Uploads/1.png");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Texture Loaded");
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Webtext = (Texture2D)myTexture;
            Debug.Log(myTexture.width);
            
        }
    }


}
