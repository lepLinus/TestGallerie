using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeGame : MonoBehaviour
{
    public int score,lives;
    public GameObject Player;
    public GameObject Astroid;
    public GameObject Ammo;
    public GameObject GamePar;
    bool started = false;
    List<GameObject> currentAstroids;

    public void StartGame()
    {
        started = true;
    }

    public void Update()
    {
        if(started)
        {
            
            Player.GetComponent<RectTransform>().position += new Vector3(Input.GetAxis("Horizontal"),0,0) * 400 * Time.deltaTime;
            Player.GetComponent<RectTransform>().localPosition = new Vector3(Mathf.Clamp(Player.GetComponent<RectTransform>().localPosition.x, -600,600), Player.GetComponent<RectTransform>().localPosition.y, 0);
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Shoot();
            }


        }
    }

    public void Shoot()
    {
        GameObject go = Instantiate(Ammo,Player.GetComponent<RectTransform>().position,Quaternion.identity);
        go.GetComponent<RectTransform>().SetParent(GamePar.transform.GetComponent<RectTransform>());
        Destroy(go,3);
    }
}
