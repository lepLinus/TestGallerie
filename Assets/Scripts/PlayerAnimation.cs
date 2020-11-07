using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    Animation anim;
    Vector3 LastPos;
    bool isvanishing = false;
    public void Start()
    {
        anim = this.GetComponent<Animation>();
    }
    public void Update()
    {
        if (this.transform.parent.GetComponent<NavMeshAgent>().velocity.magnitude > 0.1f)
        {
            if (!anim.IsPlaying("Walk"))
            {
                anim.Play("Walk");
            }
        }
        else
        {
            if (!anim.IsPlaying("Idle"))
            {
                anim.Play("Idle");
            }
        }
        if (LastPos != this.transform.position)
        {
            StartCoroutine(Display());
        }
        else if(!isvanishing)
        {
            StartCoroutine(Vanish());
        }
        LastPos = this.transform.position;
        
    }


    public IEnumerator Vanish()
    {
        isvanishing = true;
        yield return new WaitForSeconds(5);
        for (int i = 0; i< this.transform.childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    public IEnumerator Display()
    {
        isvanishing = false;
        yield return new WaitForSeconds(1);
        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}
