using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public Animator animator;

    [Header("Preferences")]
    float moveSpeed;
    public float walkSpeed;
    public float backSpeed;
    public float runSpeed;
    public float transitionSpeed;
    public float staminaIncrease;
    public float staminaDecrease;

    public bool controlled;

    public Transform waypoints;

    Vector3 direction;

    void Start()
    {
        controlled = true;

        transform.position = waypoints.GetChild(Random.Range(0, waypoints.childCount)).position;
    }

    void Update()
    {
        //If Player has no control quit
        if (!controlled)
        {
            return;
        }

        //Input
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        //Set Direction
        direction = Vector3.ClampMagnitude(transform.forward * yInput + transform.right * xInput, 1);

        //Run and Animation
        if (Input.GetKey(KeyCode.LeftShift) && yInput > 0 )
        {
            moveSpeed = Mathf.Lerp(moveSpeed, runSpeed, transitionSpeed * Time.deltaTime);
        }
        else if (yInput > 0)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, transitionSpeed * Time.deltaTime);
        }
        else if (yInput < 0)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, backSpeed, transitionSpeed * Time.deltaTime);
        }


        //Animation
        animator.SetFloat("yDir", yInput > 0 ? yInput * moveSpeed : yInput * moveSpeed * 2);
        animator.SetFloat("xDir", xInput * 5);
    }

    void FixedUpdate()
    {
        //Move
        GetComponent<Rigidbody>().MovePosition(transform.position + direction * moveSpeed * Time.fixedDeltaTime);
    }
}
