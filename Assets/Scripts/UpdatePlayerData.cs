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
    }

    public void UpdatePlayer()
    {
        StartCoroutine(UpdateP(player.exp, player.GameInfo, player.OnlineState));
    }
    public void Getscore()
    {
        StartCoroutine(GetScore());
    }
    public IEnumerator GetScore()
    {
        getrequest.Get("https://www.linuslepschies.de/PhpGallerie/GetScore.php?UserName=" + Name + "&PassWD=" + "1MRf!s13");
        yield return new WaitForSeconds(1);
        if (getrequest.Message.Length == 0 || getrequest.Message == "[]")
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
            Scoresytem.SecretsDis = new List<int>(Int32.Parse(userInfos[0].GameInfo.Split('|')[2]));
            Scoresytem.PicturesDis = new List<int>(Int32.Parse(userInfos[0].GameInfo.Split('|')[1]));
        }
    }

    public IEnumerator UpdateP(int Exp,string GameInfo,string OnlineState)
    {
        getrequest.Get("https://www.linuslepschies.de/PhpGallerie/SetScore.php?UserName=" + Name + "&PassWD=" + "1MRf!s13" + "&Exp=" + Exp + "&GameInfo=" + GameInfo + "&OnlineState=" + OnlineState);
        yield return new WaitForSeconds(1);
        if (getrequest.Message.Length == 0)
        {
        }
        else
        {
            Debug.Log("Data was updated to database");
        }
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
