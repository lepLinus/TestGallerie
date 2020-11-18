using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    
    void Update()
    {
        this.GetComponent<RectTransform>().position += new Vector3(0, 400 * Time.deltaTime, 0);
        this.GetComponent<RectTransform>().localPosition = new Vector3(this.GetComponent<RectTransform>().localPosition.x, this.GetComponent<RectTransform>().localPosition.y,0);
        if (this.GetComponent<RectTransform>().localPosition.y > 900)
        {
            Destroy(this.gameObject);
        }
    }
}
