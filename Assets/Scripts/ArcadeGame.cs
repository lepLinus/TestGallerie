using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ArcadeGame : MonoBehaviour
{
    public int score,lives;
    public GameObject player;
    public GameObject Astroid;
    public int Amount, startposx;
    public GameObject Ammo;
    public GameObject GamePar;
    bool started = false;
    List<GameObject> currentAstroids;
    public ScoreSystem MainPlayer;
    public GameObject Ammopart;
    public GameObject Brickpart;
    public TextMeshProUGUI ScoreText, LivesText;
    float countdown;
    int round;
    public GameObject Deathscreen;

    public void StartGame()
    {
        if (MainPlayer.Exp > 1000)
        {
            MainPlayer.Exp -= 1000;
        }
        else
        {
            Dead();
            return;
        }
        round = 1;
        score = 0;
        lives = 3;
        started = true;
        GamePar.SetActive(true);
        Deathscreen.SetActive(false);
        SpawnCubes();
    }

    public void Update()
    {
        if(started)
        {
            player.GetComponent<RectTransform>().position += new Vector3(Input.GetAxis("Horizontal"),0,0) * 400 * Time.deltaTime;
            countdown += Time.deltaTime;
            player.GetComponent<RectTransform>().localPosition = new Vector3(Mathf.Clamp(player.GetComponent<RectTransform>().localPosition.x, -600,600), player.GetComponent<RectTransform>().localPosition.y, 0);
            if(Input.GetKeyDown(KeyCode.Space) && countdown > 0.2f)
            {
                Shoot();
            }

            for (int j = 0; j < Brickpart.transform.childCount; j++)
            {
                for (int i = 0; i < Ammopart.transform.childCount; i++)
                {
                    if (Vector3.Distance(Ammopart.transform.GetChild(i).transform.position,Brickpart.transform.GetChild(j).transform.position) < 50)
                    {
                        Destroy(Ammopart.transform.GetChild(i).gameObject);
                        Destroy(Brickpart.transform.GetChild(j).gameObject);
                        score += 10;
                        break;
                    }
                }
                if (Vector3.Distance(player.transform.position, Brickpart.transform.GetChild(j).transform.position) < 30)
                {
                    lives--;
                    Destroy(Brickpart.transform.GetChild(j).gameObject);
                }
            }
            if (lives <= 0)
            {
                Dead();
            }
            if (Brickpart.transform.childCount < 1)
            {
                SpawnCubes();
            }
            ScoreText.text = "Score: " + score;
            LivesText.text = "Lives: " + lives;
        }
    }

    public void SpawnCubes()
    {
        round++;
        int index = 100;
        for (int i = 0; i < Amount; i++)
        {
            GameObject go = Instantiate(Astroid, Brickpart.transform.GetComponent<RectTransform>());
            go.GetComponent<RectTransform>().localPosition += new Vector3(index, 0, 0);
            go.GetComponent<Brick>().speed = round;
            index += 100;
        }
    }
    public void Dead()
    {
        MainPlayer.Exp += score;
        Deathscreen.SetActive(true);
        GamePar.SetActive(false);
        started = false;
    }
    public void Shoot()
    {
        countdown = 0;
        GameObject go = Instantiate(Ammo, player.GetComponent<RectTransform>().position,Quaternion.identity);
        go.GetComponent<RectTransform>().SetParent(Ammopart.transform.GetComponent<RectTransform>());
        Destroy(go,3);
    }
}
