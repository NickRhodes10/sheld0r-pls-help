using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomSwitch : MonoBehaviour
{
    private Animator anim;
    private bool zoomed = true;
    public GameObject minimapCamObj;
    public Camera minimapCam;
    public float zoomedInMinimapCamSize = 15f;
    public float zoomedOutMinimapCamSize = 30f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        minimapCamObj = GameObject.FindGameObjectWithTag("MinimapCam");
    }

    private void Start()
    {
        if (minimapCamObj != null && minimapCamObj.activeSelf)
        {
            minimapCam = minimapCamObj.GetComponent<Camera>();
            if (zoomed)
            {
                minimapCam.orthographicSize = zoomedInMinimapCamSize;
            }
            else
            {
                minimapCam.orthographicSize = zoomedOutMinimapCamSize;
            }            
        }
        else
        {
            Debug.Log("MinimapCam either null or not active.");
        }
    }

    public void SwitchState()
    {
        if (zoomed)
        {
            anim.Play("Zoomed Out");
            if (minimapCam != null)
            {                
                minimapCam.orthographicSize = zoomedOutMinimapCamSize;
            }
        }
        else
        {
            anim.Play("Zoomed In");
            if (minimapCam != null)
            {
                minimapCam.orthographicSize = zoomedInMinimapCamSize;
            }                           
        }

        zoomed = !zoomed;
    }
}

