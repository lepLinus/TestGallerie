using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public CharacterController controller;
    public Transform playerCamera;

    public float gravity = -13.0f;
    public float walkSpeed = 6.0f;
    public float mouseSensitivity = 3.5f;
    [Range(0.0f, 0.5f)] public float moveSmoothTime = 0.2f;
    [Range(0.0f, 0.5f)] public float mouseSmoothTime = 0.03f;
    
    float cameraPitch = 0.0f;
    float velocityY = 0.0f;

    Vector2 currentDir, currentDirVelocity;
    Vector2 currentMouseDelta, currentMouseDeltaVelocity;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        UpdateMovement();
        UpdateMouseLook();
    }

    void UpdateMovement()
    {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        if (controller.isGrounded)
        {
            velocityY = 0.0f;
        }

        velocityY += gravity * Time.deltaTime;

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * walkSpeed + Vector3.up * velocityY;
        controller.Move(velocity * Time.deltaTime);
    }

    void UpdateMouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);
        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90, 90);

        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
    }
}
