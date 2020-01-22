using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformSync : MonoBehaviour
{
    public GameObject Player, OtherPlayer;

    void Update()
    {
        OtherPlayer.transform.position = new Vector3(Player.transform.position.x, OtherPlayer.transform.position.y, Player.transform.position.z);
        OtherPlayer.transform.rotation = Player.transform.rotation;
    }
}
