using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public float speed;
    void Update()
    {
        this.GetComponent<RectTransform>().position += new Vector3(0, -40 * speed/5 * Time.deltaTime, 0);
        if (this.GetComponent<RectTransform>().localPosition.y < -300)
        {
            Destroy(this.gameObject);
        }
    }
}