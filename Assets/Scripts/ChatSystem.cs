using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ChatSystem : MonoBehaviour
{
    // Start is called before the first frame update
    string Message;
    public void SendChat()
    {

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
                Message = www.downloadHandler.text;
            }
            Debug.Log(":\nReceived: " + www.downloadHandler.text);
        }

    }

}
