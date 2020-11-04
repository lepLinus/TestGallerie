using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Security.Cryptography;
using System.Text;
using System;

public class Login : MonoBehaviour
{
    Getrequest getrequest;
    [Header("Input Field for Login")]
    public TMP_InputField NameInput,PasswdInput;
    [Header("Text for Error and Debug on User UI")]
    public TextMeshProUGUI ErrorText;

    void Start()
    {
        getrequest = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<Getrequest>();
    }
    public void StartLogin()
    {
        StartCoroutine(LoginSend());
    }
    // Update is called once per frame
    public IEnumerator LoginSend() {
        string Name = NameInput.text;
        string Passwort = PasswdInput.text;
       
        if (Name == "" || Name == null)
        {
            ErrorText.text = "Name can not be empty";

        }else if (Passwort == "" || Passwort == null)
        {
            ErrorText.text = "Password can not be empty";
        }
        
       // Passwort = GetMD5(Passwort);
        getrequest.Get("https://www.linuslepschies.de/PhpGallerie/login.php?Name=" + Name + "&PassWD="+ Passwort);
        yield return new WaitForSeconds(2);
        if (getrequest.Message.Length == 0)
        {
            ErrorText.text = "Error on Login" + getrequest.Message;
        }
        else
        {
            GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>().Login(Name);
            this.gameObject.SetActive(false);
        }
    }

    public static string GetMD5(string text)
    {
        byte[] textBytes = Encoding.UTF8.GetBytes(text);
        byte[] hash = SHA1.Create().ComputeHash(textBytes);
        
        return Convert.ToBase64String(hash);
    }
}
