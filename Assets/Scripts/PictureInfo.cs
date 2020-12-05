using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class PictureInfo : MonoBehaviour
{
    public TextMeshProUGUI Header, MainText;
    public RawImage Priv;
    public Texture Image;
    string Message;
    public int pictureid;
    string link;
    public void SetNewText(string picturename)
    {
        pictureid = Int32.Parse(picturename);
        StartCoroutine(GetText());
    }
    public void OpenLink()
    {
        Application.OpenURL(link);
    }
    public IEnumerator GetText()
    {
        StartCoroutine(GetRequest("https://www.linuslepschies.de/PhpGallerie/GetPictureData.php?PassWD=1MRf!s13&Id=" + pictureid));
        yield return new WaitForSeconds(0.5f);
        if (Message.Length == 0 || Message == "[]")
        {
            Debug.Log("Error on Getting data");
        }
        else
        {
            string josn = "{\"Items\":" + Message + "}";
            Pictureinfo[] pictureinfos = JsonHelper.FromJson<Pictureinfo>(josn);
            Debug.Log(" Pic info "+ pictureinfos.Length + "  "  + pictureid);
            for (Int32 i = 0; i < pictureinfos.Length; i++)
            {
                Debug.Log("For ");
                if (i == pictureid-1)
                {
                    Debug.Log("if");
                    MainText.text = pictureinfos[i].PictureDes;
                    Header.text = pictureinfos[i].ExhabName + " - " + pictureinfos[i].ArtName;
                    link = pictureinfos[i].SocialMedia;
                    break;
                }
            }
            StartCoroutine(GetTexture());
        }
    }
    IEnumerator GetTexture()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://www.linuslepschies.de/GalleriePictures/Uploads/" + pictureid + ".png");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Image = myTexture;
            Priv.texture = Image;
        }
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
                Message = webRequest.downloadHandler.text;
            }
        }
    }

    [System.Serializable]
    public class Pictureinfo
    {
        public string ExhabName;
        public string PictureDes;
        public string Location;
        public string ArtName;
        public string SocialMedia;

        public static Pictureinfo CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<Pictureinfo>(jsonString);
        }
    }

}
