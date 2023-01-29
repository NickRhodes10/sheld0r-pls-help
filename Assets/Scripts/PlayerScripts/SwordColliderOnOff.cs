using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordColliderOnOff : MonoBehaviour
{
    public MeshCollider swordCollider;
    public BoxCollider swordColliderBox;

    public bool useBoxCollider;

    public void EnableSwordCollider()
    {
        if (useBoxCollider)
        {
            swordColliderBox.enabled = true;
        }
        else
        {
            swordCollider.enabled = true;
        }
    }

    public void DisableSwordCollider()
    {
        if (useBoxCollider)
        {
            swordColliderBox.enabled = false;
        }
        else
        {
            swordCollider.enabled = false;
        }
    }
}
