using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class RankSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public string Message;
    public GameObject Menu;
    public TextMeshProUGUI RankNames;
    public TextMeshProUGUI Exp;

    public void Start()
    {
        Message = null;
    }

    public void Open()
    {
        Menu.SetActive(true);
        StartCoroutine(OpenInner());
    }

    public IEnumerator OpenInner()
    {
        Message = null;
        StartCoroutine(GetRequest("https://www.linuslepschies.de/PhpGallerie/Rank.php?PassWD=1MRf!s13"));
        yield return new WaitForSeconds(1);
        if (Message.Length == 0 || Message == "[]")
        {
            Debug.Log("Error on Getting data");
        }
        else
        {
            string josn = "{\"Items\":" + Message + "}";
            RankInfo[] userInfos = JsonHelper.FromJson<RankInfo>(josn);
            RankNames.text = "";
            Exp.text = "";
            for (int i = 0; i< userInfos.Length; i++)
            {
                RankNames.text += userInfos[i].UserName + "\n" + "\n";
                Exp.text += userInfos[i].Exp + " Inspiration" + "\n" + "\n";
            }
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
                try
                {
                    //Message = System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data, 3, webRequest.downloadHandler.data.Length - 3);
                    Message = webRequest.downloadHandler.text;
                }
                catch (Exception e)
                {
                    Message = webRequest.downloadHandler.text;
                }
            }
        }
    }

    [System.Serializable]
    public class RankInfo
    {
        public string UserName;
        public string Exp;
        public static RankInfo CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<RankInfo>(jsonString);
        }
    }
}
