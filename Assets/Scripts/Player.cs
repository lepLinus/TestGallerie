using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int exp;
    public string name;
    public ScoreSystem Scoresytem;
    void Update()
    {
        Scoresytem.Exp = exp;
    }
}
