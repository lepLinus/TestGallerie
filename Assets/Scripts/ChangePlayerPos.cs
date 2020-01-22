using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ChangePlayerPos : MonoBehaviour
{
    public GameObject OtherPlayer;

    public void OnTriggerStay(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.tag == "Player")
        {
            OtherPlayer.GetComponent<TransformSync>().enabled = false;
            Vector3 tmppos = other.transform.position;
            other.transform.position = OtherPlayer.transform.position;
            OtherPlayer.transform.position = tmppos;
            OtherPlayer.GetComponent<TransformSync>().enabled = true;
        }
    }
}
