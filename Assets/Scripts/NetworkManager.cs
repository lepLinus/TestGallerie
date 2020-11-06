﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour
{
    public GameObject PlayerPref;
    public GameObject DisplayText;
    public string getposurl;
    public string getuserurl;
    public Getrequest getrequest;
    int userid;
    int exp;
    string Progress;
    bool Playercreated = false;
    public static string Name;

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1 && !Playercreated)
        {
            Playercreated = true;
            PlayerPref = GameObject.FindGameObjectWithTag("Player");
            PlayerPref.GetComponent<Player>().Name = Name;
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

    public void Login(string name)
    {
        Name = name;
        DisplayText.GetComponent<DisplayText>().StartDisplaytext(name);
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
