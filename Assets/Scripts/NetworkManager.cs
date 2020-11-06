using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour
{
    public GameObject PlayerPref;
    public GameObject DisplayText;
    public Getrequest getrequest;

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


    public void Login(string name)
    {
        Name = name;
        DisplayText.GetComponent<DisplayText>().Maintext.transform.parent.gameObject.SetActive(true);
        DisplayText.GetComponent<DisplayText>().StartDisplaytext(name);
    }

}
