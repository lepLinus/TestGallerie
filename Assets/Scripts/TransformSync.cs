using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformSync : MonoBehaviour
{
    public GameObject Player, OtherPlayer;

    void Update()
    {
        this.transform.position = new Vector3(Player.transform.position.x, this.transform.position.y, Player.transform.position.z);
        this.transform.rotation = Player.transform.rotation;
    }
}
