using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DisplayText : MonoBehaviour
{
    public TextMeshProUGUI Maintext;
    public string[] Alldialogs;

    int index;
    bool isrunning = true;
    public GameObject PressKeytext;


    // Start is called before the first frame update
    void Start()
    {
        index = 0;
    }

    public void StartDisplaytext(string name)
    {
        if (name == "")
        {
            name = "User";
        }
        Alldialogs[0] = "Hey " + name;
        StartCoroutine(Displaytext(Alldialogs[index]));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && !isrunning)
        {
            isrunning = true;
            Maintext.text = "";
            PressKeytext.SetActive(false);
            index++;
            if (index >= Alldialogs.Length)
            {
                Maintext.text = "Loading...";
                SceneManager.LoadScene(1);
            }
            else
            {
                StartCoroutine(Displaytext(Alldialogs[index]));
            } 
        }
    }

    public IEnumerator Displaytext(string text)
    {
        
        char[] c = text.ToCharArray();
        for (int i = 0; i< c.Length; i++)
        {
            Maintext.text += c[i];
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1);
        PressKeytext.SetActive(true);
        isrunning = false;
    }
}
