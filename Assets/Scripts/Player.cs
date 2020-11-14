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
    [HideInInspector]
    public int CountofPicDis;
    public int CountofSecDis;

    public void Start()
    {
        Name = NetworkManager.Name;
        StartCoroutine(Updatetoserver(true));
    }

    void Update()
    {
        exp = Scoresytem.Exp;
        OnlineState = "true";
        CountofPicDis = Scoresytem.PicturesDis.Count;
        CountofSecDis = Scoresytem.SecretsDis.Count;

        string x = transform.position.x.ToString("0.00");
        string y = transform.position.y.ToString("0.00");
        string z = transform.position.z.ToString("0.00");

        GameInfo = x.Replace('.',',') + ";" + y.Replace('.', ',') + ";" + z.Replace('.', ',') + "|" + CountofPicDis + "|" + CountofSecDis;
    }

    public IEnumerator Updatetoserver(bool isfirstrun)
    {
        if (isfirstrun)
        {
            updatePlayerData.Getscore();
            yield return new WaitForSeconds(1.1f);
        }
        updatePlayers.LoadAll();
        yield return new WaitForSeconds(1.1f);
        updatePlayerData.UpdatePlayer();
        yield return new WaitForSeconds(1.1f);
        updatePlayers.LoadAll();
        yield return new WaitForSeconds(1.1f);
        StartCoroutine(Updatetoserver(false));
    }

}
