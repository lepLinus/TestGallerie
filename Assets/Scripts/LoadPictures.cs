using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class LoadPictures : MonoBehaviour
{
    public string getpicURLurl;
    public Getrequest getrequest;
    int userid;
    public GameObject[] Pictures;
    public string[] descriptions;
    public string[] socialmedia;
    public string[] NameofArt;

    public IEnumerator UpdateArt()
    {
        getrequest.Get(getpicURLurl);
        while (getrequest.Message == null)
        {
            yield return new WaitForSeconds(0.1f);
        }

        string josn = "{\"Items\":" + getrequest.Message + "}";
        Debug.Log(josn);
        PictureInfo[] pictureInfo = JsonHelper.FromJson<PictureInfo>(josn);

        for (int i = 0; i< pictureInfo.Length; i++)
        {
            string[] weblinksroot = pictureInfo[i].PictureDes.Split('|');
            for (int j = 0; j < weblinksroot.Length; j++)
            {
                string[] split = weblinksroot[j].Split(';');
                StartCoroutine(GetTexture(split[0],Pictures[Int32.Parse(split[1])]));
            }
        }
        yield return new WaitForSeconds(1);
    }

    IEnumerator GetTexture(string URL, GameObject Picture)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(URL);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Picture.GetComponent<MeshRenderer>().material.SetTexture(0,myTexture);
        }
    }

    [System.Serializable]
    public class PictureInfo
    {
        public string PictureLink;
        public string PictureDes;
        public string ArtName;
        public string SocialMedia;
        public string ExhabName;
        public static PictureInfo CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<PictureInfo>(jsonString);
        }
    }
}

