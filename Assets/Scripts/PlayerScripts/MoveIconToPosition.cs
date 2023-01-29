using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveIconToPosition : MonoBehaviour
{
    public Transform objectToMoveTo;
    public float yLevel;

    private void Start()
    {
        if (objectToMoveTo == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            objectToMoveTo = player.transform;
        }
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(objectToMoveTo.position.x, yLevel, objectToMoveTo.position.z);
    }
}
