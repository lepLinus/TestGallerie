using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class Getrequest : MonoBehaviour
{
    public string Message;

    public void Start()
    {
        Message = null;
    }

    public void Get(string URL)
    {
        Message = null;
        StartCoroutine(GetRequest(URL));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            webRequest.certificateHandler = new AcceptAllCertificatesSignedWithASpecificKeyPublicKey();
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                try
                {
                    //Message = System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data, 3, webRequest.downloadHandler.data.Length - 3);
                    Message = webRequest.downloadHandler.text;
                }
                catch (Exception e)
                {
                    Message = webRequest.downloadHandler.text;
                }
                Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
            }
        }
    }
}