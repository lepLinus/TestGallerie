using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ChangePlayerPos : MonoBehaviour
{
    public GameObject OtherPlayer;
    
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if(other.gameObject.tag == "Player")
        {
            //OtherPlayer.GetComponent<TransformSync>().enabled = false;
            Vector3 tmppos = OtherPlayer.transform.position;
            other.gameObject.transform.position = OtherPlayer.transform.position;
            OtherPlayer.transform.position = tmppos;
            OtherPlayer.GetComponent<TransformSync>().enabled = true;
        }
    }
}
