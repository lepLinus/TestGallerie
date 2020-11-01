using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScoreSystem : MonoBehaviour
{
    public int Exp;
    public int PictureScore;
    Animation Scoreanim;
    public TextMeshProUGUI Scoretext, ExpText;
    List<int> PicturesDis = new List<int>(); 
    void Start()
    {
        Scoreanim = this.GetComponent<Animation>();
    }

    // Update is called once per frame
    public void Update()
    {
        ExpText.text = "Exp: " + Exp;
    }
    public void AddScorePicture(int index)
    {
        for (int i = 0; i< PicturesDis.Count; i++)
        {
            if (PicturesDis[i] == index)
            {
                return;
            }
        }
        PicturesDis.Add(index);
        Scoreanim.Play("Fadein");
        Scoretext.text = (PicturesDis.Count.ToString()+1) + "/19 Pictures Discovered \n Exp +" + PictureScore;
        Exp += PictureScore;
    }
}
