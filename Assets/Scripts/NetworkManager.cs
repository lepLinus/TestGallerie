using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public GameObject PlayerPref;
    public string getposurl;
    public Getrequest getrequest;
    int userid;
    

    public IEnumerator UpdatePos()
    {
        getrequest.Get(getposurl + userid);
        while (getrequest.Message == null)
        {
            yield return new WaitForSeconds(0.1f);
        }

        string josn = "{\"Items\":" + getrequest.Message + "}";
        Debug.Log(josn);
        UserInfo[] userInfo = JsonHelper.FromJson<UserInfo>(josn);
        
        
        yield return new WaitForSeconds(1);
    }

    [System.Serializable]
    public class MessageInfo
    {
        public string Id, UserName, Company, Message, Date;

        public static MessageInfo CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<MessageInfo>(jsonString);
        }
    }
    [System.Serializable]
    public class UserInfo
    {
        public string Company;

        public static UserInfo CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<UserInfo>(jsonString);
        }
    }
}
