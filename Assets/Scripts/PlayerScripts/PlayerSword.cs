using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    [SerializeField] public int damageAmount = 1;                      // Weapon damage

    private void Start()
    {
        this.GetComponent<MeshCollider>().enabled = false;             // Keeps the collider from activating unless told to by the animation events, set in PlayerLocomotion
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy")) // If we have collided with an enemy,
        {
            var hittable = other.GetComponent<IHittable>();        // Grab the IHittable component
            if (hittable != null)
            {
                hittable.GetHit(damageAmount);                         // Call GetHit function passing damageAmount
                Debug.Log("Hit enemy");
            }

        }
    }
}
