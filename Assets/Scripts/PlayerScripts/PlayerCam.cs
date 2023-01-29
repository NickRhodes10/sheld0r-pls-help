using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManager;

public class PlayerCam : MonoBehaviour
{
    public float sensitivityX;
    public float sensitivityY;

    public Transform camHolder; // Transform of the Camera Holder object   
    public Transform playerModel;

    float xRotation;
    float yRotation; // floats storing the current x and y rotation of the camera


    private void Start()
    {
        PlayerManager.pm.DisableCursor();

        camHolder = MoveCamera.instance.transform;
        playerModel = Hotbar.instance.GetComponent<Transform>();

        xRotation = 0;
        yRotation = 0;
    }

    private void FixedUpdate()
    {
        if (PlayerManager.pm.gameplayPaused == false)
        {
            // Get mouse input
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivityX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivityY;

            // Assign Rotation Variables Based On Mouse Input
            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 70f);

            // Rotate Cam and Orientation
            camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            playerModel.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }
}
