using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class UpdatePlayers : MonoBehaviour
{
    public GameObject PlayerNotLocalPref;
    Getrequest getrequest;
    public GameObject PlayersParent;
    public Player player;

    public void Start()
    {
        getrequest = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<Getrequest>();
    }

    public void LoadAll()
    {
        StartCoroutine(Load());
    }
    /*
    public IEnumerator UpdatePos()
    {
        getrequest.Get("https://www.linuslepschies.de/PhpGallerie/GetAllPos.php?PassWD=1MRf!s13");
        yield return new WaitForSeconds(1);
        if (getrequest.Message.Length == 0)
        {
            Debug.Log("Error on Getting data");
        }
        else
        {
            string josn = "{\"Items\":" + getrequest.Message + "}";
            UserInfo[] userInfos = JsonHelper.FromJson<UserInfo>(josn);
            string[] Allpos = new string[userInfos.Length];

            for (int i = 0; i < userInfos.Length; i++)
            {
                if (userInfos[i].GameInfo.Split('|')[userInfos[i].GameInfo.Split('|').Length - 1] == "offline")
                {
                    continue;
                }
                Allpos[i] = userInfos[i].GameInfo.Split('|')[0];
                PlayersParent.transform.GetChild(i).gameObject.GetComponent<NavMeshAgent>().destination = new Vector3(float.Parse(Allpos[i].Split(',')[0]), float.Parse(Allpos[i].Split(',')[1]), float.Parse(Allpos[i].Split(',')[2]));
            }
        }
    }*/


    public IEnumerator Load()
    {
        getrequest.Get("https://www.linuslepschies.de/PhpGallerie/GetAllPos.php?PassWD=1MRf!s13");
        yield return new WaitForSeconds(1);
        if (getrequest.Message.Length == 0)
        {
            Debug.Log("Error on Getting data");
        }
        else
        {
            string josn = "{\"Items\":" + getrequest.Message + "}";
            UserInfo[] userInfos = JsonHelper.FromJson<UserInfo>(josn);
            string[] Allpos = new string[userInfos.Length];

            for (int i = 0;i < userInfos.Length; i++)
            {
                bool isspwned = false;

                for (int j = 0; j< PlayersParent.transform.childCount; j++)
                {
                    if (PlayersParent.transform.GetChild(j).name == userInfos[i].UserName)
                    {
                        Allpos[i] = userInfos[i].GameInfo.Split('|')[0];
                        PlayersParent.transform.GetChild(i).gameObject.GetComponent<NavMeshAgent>().destination = new Vector3(float.Parse(Allpos[i].Split(';')[0]), float.Parse(Allpos[i].Split(';')[1]), float.Parse(Allpos[i].Split(';')[2]));
                        isspwned = true;
                        break;
                    }
                }

                if (userInfos[i].UserName == player.Name || userInfos[i].GameInfo.Split('|')[userInfos[i].GameInfo.Split('|').Length-1] == "offline" || isspwned == true)
                {
                    continue;
                }

                Allpos[i] = userInfos[i].GameInfo.Split('|')[0];
                GameObject go = Instantiate(PlayerNotLocalPref,new Vector3(float.Parse(Allpos[i].Split(';')[0]), float.Parse(Allpos[i].Split(';')[1]), float.Parse(Allpos[i].Split(';')[2])),Quaternion.identity);
                go.transform.parent = PlayersParent.transform;
                go.name = userInfos[i].UserName;
            }
        }
    }

    [System.Serializable]
    public class UserInfo
    {
        public string GameInfo;
        public string UserName;
        public static UserInfo CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<UserInfo>(jsonString);
        }
    }
}
