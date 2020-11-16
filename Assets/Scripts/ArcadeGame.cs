﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeGame : MonoBehaviour
{
    public int score,lives;
    public GameObject Player;
    public GameObject Astroid;
    public GameObject Ammo;
    bool started = false;

    public void StartGame()
    {
        started = true;
    }

    public void Update()
    {
        if(started)
        {
            
            Player.GetComponent<RectTransform>().position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"),0) * 200 * Time.deltaTime;
            Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("MousePos" + Input.mousePosition);
            Debug.Log("Mouse Pos" + mouseScreenPosition);
            // get direction you want to point at
            //Vector2 direction = (mouseScreenPosition - (Vector2)Player.transform.position).normalized;
            //Debug.Log("dir" + direction);
            // set vector of transform directly
            //Player.transform.up = direction;
            Player.transform.LookAt(mouseScreenPosition, Vector3.right);
        }
    }
}
