using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour
{
    public GameObject PlayerPref;
    public string getposurl;
    public string getuserurl;
    public Getrequest getrequest;
    int userid;
    int exp;
    string Progress;
    public TextMeshProUGUI ErrorText;
    public TMP_InputField NameInput, PassWdInput;
    bool Playercreated = false;

    public void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1 && !Playercreated)
        {
            Playercreated = true;
            GameObject go = Instantiate(PlayerPref,new Vector3(3.07f, 0.745f, 19.892f),Quaternion.identity);
            go.GetComponent<Player>().exp = exp;
        }
    }

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

    public void Login()
    {
        string Name = NameInput.text;
        string Passwd = PassWdInput.text;

    }


    public IEnumerator Login(string loginstring)
    {
        getrequest.Get(getposurl + loginstring);
        while (getrequest.Message == null)
        {
            yield return new WaitForSeconds(0.1f);
        }
        if (getrequest.Message == "Error")
        {
            ErrorText.text = "Error on Login";
        }
        else
        {
            string josn = "{\"Items\":" + getrequest.Message + "}";
            Debug.Log(josn);
            UserInfo[] userInfo = JsonHelper.FromJson<UserInfo>(josn);
            exp = Int32.Parse(userInfo[0].Exp);
            Progress = userInfo[0].GameInfo;
        }
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
        public string Id;
        public string UserName;
        public string PassWD;
        public string Email;
        public string Exp;
        public string GameInfo;
        public static UserInfo CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<UserInfo>(jsonString);
        }
    }
}
