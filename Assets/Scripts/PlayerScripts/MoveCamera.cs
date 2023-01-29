using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    // This script is just used to allow us to change the CameraPos object independently of any other objects

    public static MoveCamera instance;

    private void Awake()
    {
        if (MoveCamera.instance == null)
        {
            MoveCamera.instance = this;
        }
        else if (MoveCamera.instance != this)
        {
            Destroy(this);
        }
    }

    public Transform cameraPosition;

    private void Start()
    {
        cameraPosition = CameraPosition.instance.transform;
    }

    private void Update()
    {
        transform.position = cameraPosition.position;
    }
}
