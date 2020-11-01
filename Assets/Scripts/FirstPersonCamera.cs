using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public Transform camera;
    public float sensitivity;
    float inputX, inputY;
    public void Start()
    {
       // inputY = camera.eulerAngles.y;
       // inputX = camera.eulerAngles.x;
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            inputX += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            inputY -= Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
            inputY = Mathf.Clamp(inputY, -90, 90);
            // Cursor.visible = false;
            Cursor.visible = true;
           // Cursor.lockState = CursorLockMode.Locked;
            camera.localEulerAngles = new Vector3(inputY, inputX, 0);
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
