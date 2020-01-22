using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    CharacterController controller;

    public GameObject Camera;
    public GameObject Body;

    float Speed;
    public float WalkSpeed;
    public float RunSpeed;
    public float SpeedChange;
    public float Sensitivity;
    float xAxisClamp;
    float yAxisClamp;

    public float GroundCheckRadius;

    public AnimationCurve JumpFalloff;
    public float JumpMultiplier;
    bool IsJumping;
    bool JumpButtonPressed;

    Vector3 MoveDirection;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //Input Setup
        float InputX = Input.GetAxis("Horizontal");
        float InputY = Input.GetAxis("Vertical");
        float MouseX = Input.GetAxis("Mouse X");
        float MouseY = Input.GetAxis("Mouse Y");
        xAxisClamp -= (MouseY * Sensitivity * Time.deltaTime) / Time.timeScale;
        yAxisClamp += (MouseX * Sensitivity * Time.deltaTime) / Time.timeScale;

        MoveDirection = transform.right * InputX + transform.forward * InputY;


        //Run
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Speed = Mathf.Lerp(Speed, RunSpeed, SpeedChange);
        }
        else
        {
            Speed = Mathf.Lerp(Speed, WalkSpeed, SpeedChange);
        }


        //Movement
        controller.SimpleMove(MoveDirection * Speed);


        //Rotation
        xAxisClamp = Mathf.Clamp(xAxisClamp, -90, 90);

        Camera.transform.localEulerAngles = new Vector3(xAxisClamp, 0, 0);
        transform.eulerAngles = Vector3.up * yAxisClamp;

        //Jump
        JumpInput();
    }

    void JumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !IsJumping)
        {
            IsJumping = true;
            StartCoroutine(JumpEvent());
        }
    }

    IEnumerator JumpEvent()
    {
        controller.slopeLimit = 90;
        float TimeInAir = 0;

        do
        {
            float JumpForce = JumpFalloff.Evaluate(TimeInAir);
            float InputLR = Input.GetAxis("Horizontal");

            Vector3 MoveLR = transform.right * InputLR;

            controller.Move(MoveLR * Speed * Time.deltaTime + (Vector3.up * JumpForce * JumpMultiplier * Time.deltaTime));
            TimeInAir += Time.deltaTime;

            yield return null;
        }
        while (!controller.isGrounded && controller.collisionFlags != CollisionFlags.Above);
        IsJumping = false;
        controller.slopeLimit = 45;
    }
}
