using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayText : MonoBehaviour
{
    public TextMeshProUGUI Maintext;
    public string[] Alldialogs;
    int index;
    bool isrunning;
    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        StartCoroutine(Displaytext(Alldialogs[index]));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && !isrunning)
        {
            isrunning = true;
            Maintext.text = "";
            index++;
            StartCoroutine(Displaytext(Alldialogs[index]));
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
        yield return new WaitForSeconds(2);
        Maintext.text = "";
        isrunning = false;
    }
}
