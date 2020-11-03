using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
            ErrorText.text = "Name kann nicht leer sein";

        }else if (Passwort == "" || Passwort == null)
        {
            ErrorText.text = "Passwort kann nicht leer sein";
        }
        getrequest.Get("https://linuslepschies.de/login.php?Name=" + Name + "&PassWD="+ Passwort.GetHashCode());
        yield return new WaitForSeconds(2);
        if (getrequest.Message.Length == 0)
        {
            ErrorText.text = "Fehler beim anmelden" + getrequest.Message;
        }
        else
        {
            GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>().Login(Name);
        }
    }
}
