using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int exp;
    public string Name;
    public ScoreSystem Scoresytem;
    public string GameInfo;
    public UpdatePlayerData updatePlayerData;

    public void Start()
    {
        Name = NetworkManager.Name;
        updatePlayerData.UpdatePlayer(exp, GameInfo);
        
    }

    void LateUpdate()
    {
        exp = Scoresytem.Exp;
        GameInfo = transform.position.x + "," + transform.position.y + "," + transform.position.z + "|" + "online";
    }

}
