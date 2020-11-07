using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    Animation anim;

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

    }

}
