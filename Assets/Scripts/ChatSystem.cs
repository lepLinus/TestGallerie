using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class ChatSystem : MonoBehaviour
{
    // Start is called before the first frame update
    string Message;
    public TMP_InputField textinput;
    public TextMeshProUGUI ChatText;
    public Player player;

    public void SendChat()
    {
        string message = textinput.text;
        if (!CheckforLoginState())
        {
            return;
        }
        if (message == "")
        {
            return;
        }
        StartCoroutine(PostRequest("https://www.linuslepschies.de/PhpGallerie/SendChat.php","UserName=" + player.Name + "&Message=" + message));
        textinput.text = "";
    }

    public IEnumerator GetChat()
    {
        StartCoroutine(GetRequest("https://www.linuslepschies.de/PhpGallerie/GetChat.php?PassWD=1MRf!s13"));
        yield return new WaitForSeconds(0.5f);
        if (Message.Length == 0 || Message == "[]")
        {
            Debug.Log("Error on Getting data");
        }
        else
        {
            string josn = "{\"Items\":" + Message + "}";
            Chat[] userInfos = JsonHelper.FromJson<Chat>(josn);
            ChatText.text = "";
            for (int i = 0; i< userInfos.Length; i++)
            {
                ChatText.text += userInfos[i].UserName + " : " + userInfos[i].Message + "\n";
            }
        }
        StartCoroutine(GetChat());
    }

    public void CloseChat()
    {
        StopAllCoroutines();
    }
    public void OpenChat()
    {
        if (!CheckforLoginState())
        {
            return;
        }
        StartCoroutine(GetChat());
    }


    public bool CheckforLoginState()
    {
        try
        {
            if (player.Name.Length < 1 || player.Name == null)
            {
                StopAllCoroutines();
                ChatText.text = "Login to read Chat!";
                return false;
            }
        }
        catch (System.Exception)
        {
            StopAllCoroutines();
            ChatText.text = "Login to read Chat!";
            return false;
        }
        return true;
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

    IEnumerator PostRequest(string URL, string data)
    {
        //field1=foo&field2=bar
        WWWForm form = new WWWForm();
        string[] split = data.Split('&');

        for (int i = 0; i < split.Length; i++)
        {
            form.AddField(split[i].Split('=')[0], split[i].Split('=')[1]);
        }

        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
               // Message = www.downloadHandler.text;
            }
            Debug.Log(":\nReceived: " + www.downloadHandler.text);
        }
    }
    [System.Serializable]
    public class Chat
    {
        public string UserName;
        public string Message;
        public static Chat CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<Chat>(jsonString);
        }
    }
}
