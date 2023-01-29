using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    public static CameraPosition instance;
    private void Awake()
    {
        if (CameraPosition.instance == null)
        {
            CameraPosition.instance = this;
        }
        else if (CameraPosition.instance != this)
        {
            Destroy(this);
        }
    }
}
