using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelBreaking : MonoBehaviour, IHittable
{
      int barrelHealth = 1;

      public void GetHit(int damage)
      {
            barrelHealth -= damage;
            if(barrelHealth <= 0)
            {
                  Destroy(gameObject);
            }
      }

      public void GetStunned(float length)
      {
            return;
      }

}
