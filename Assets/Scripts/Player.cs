using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int exp;
    public string Name,OnlineState;
    public ScoreSystem Scoresytem;
    public string GameInfo;
    public UpdatePlayerData updatePlayerData;
    public UpdatePlayers updatePlayers;

    public void Start()
    {
        Name = NetworkManager.Name;
        StartCoroutine(Updatetoserver(true));
    }

    void Update()
    {
        exp = Scoresytem.Exp;
        OnlineState = "true";
        GameInfo = transform.position.x.ToString("0.00") + ";" + transform.position.y.ToString("0.00") + ";" + transform.position.z.ToString("0.00") + "|";
    }

    public IEnumerator Updatetoserver(bool isfirstrun)
    {
        if (isfirstrun)
        {
            updatePlayerData.Getscore();
            yield return new WaitForSeconds(1.5f);
        }
        updatePlayerData.UpdatePlayer();
        yield return new WaitForSeconds(1.5f);
        updatePlayers.LoadAll();
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(Updatetoserver(false));
    }

}
