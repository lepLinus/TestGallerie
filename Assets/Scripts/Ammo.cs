using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
   
    void Update()
    {
        this.GetComponent<RectTransform>().position += new Vector3(0, 400 * Time.deltaTime, 0);
    }
}
