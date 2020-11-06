using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UpdatePlayerData : MonoBehaviour
{
    Getrequest getrequest;
    public string Name;
    public Player player;
    public ScoreSystem Scoresytem;

    public void Start()
    {
        getrequest = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<Getrequest>();
        Name = NetworkManager.Name;
        StartCoroutine(GetPos());
    }

    public void UpdatePlayer(int Exp,string GameInfo)
    {
        StartCoroutine(UpdateOuter(Exp,GameInfo));
    }

    public IEnumerator UpdateOuter(int Exp, string GameInfo)
    {
        yield return new WaitForSeconds(2.1f);
        StartCoroutine(UpdateP(Exp, GameInfo));
        yield return new WaitForSeconds(5);
        UpdatePlayer(player.exp,player.GameInfo);
    }

    public IEnumerator GetPos()
    {
        getrequest.Get("https://www.linuslepschies.de/PhpGallerie/GetScore.php?UserName=" + Name + "&PassWD=" + "1MRf!s13");
        yield return new WaitForSeconds(2);
        if (getrequest.Message.Length == 0)
        {
            Debug.Log("Error on Getting data");
        }
        else
        {
            string josn = "{\"Items\":" + getrequest.Message + "}";
            UserInfo[] userInfos = JsonHelper.FromJson<UserInfo>(josn);
            player.exp = Int32.Parse(userInfos[0].Exp);
            Scoresytem.Exp = Int32.Parse(userInfos[0].Exp);
            player.GameInfo = userInfos[0].GameInfo;
        }
    }
    public IEnumerator UpdateP(int Exp,string GameInfo)
    {
        getrequest.Get("https://www.linuslepschies.de/PhpGallerie/SetScore.php?UserName=" + Name + "&PassWD=" + "1MRf!s13" + "&Exp=" + Exp + "&GameInfo=" + GameInfo);
        yield return new WaitForSeconds(2);
        if (getrequest.Message.Length == 0)
        {
        }
        else
        {
            Debug.Log("Data was updated to database");
        }
    }

    public void OnApplicationQuit()
    {
        StartCoroutine(UpdateP(player.exp,player.GameInfo.Split('|')[0] + "|offline"));
    }


    [System.Serializable]
    public class UserInfo
    {
        public string Exp;
        public string GameInfo;
        public static UserInfo CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<UserInfo>(jsonString);
        }
    }
}
