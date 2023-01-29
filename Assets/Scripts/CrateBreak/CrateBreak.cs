using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateBreak : MonoBehaviour, IHittable
{
    public int crateHealth = 1;

    [SerializeField] bool isBroken;

    ResourceDropper resourceDropper;

    private void Start()
    {
        resourceDropper = GetComponentInChildren<ResourceDropper>();
    }

    private void Update()
    {
        if (isBroken)
        {
            Break();
        }
    }

    public void GetHit(int damage)
    {
        Debug.Log("Crate Hit");

        if (isBroken == false)
        {
            crateHealth -= damage;

            if (crateHealth <= 0)
            {
                isBroken = true;
            }
        }
    }

    private void Break()
    {
        resourceDropper.DropItem();
        Destroy(gameObject);
    }

    public void GetStunned(float length)
    {
        return;
    }
}
