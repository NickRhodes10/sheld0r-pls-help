using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class ProjectileBase : MonoBehaviour
{
    protected Transform _source;

    public Transform Source { get; set; }

    private void OnEnable()
    {
        if(GetComponent<Rigidbody>() == null)
        {
            gameObject.AddComponent<Rigidbody>();
        }

        GetComponent<Rigidbody>().useGravity = false;
    }
}
