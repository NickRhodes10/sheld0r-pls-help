using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
      [SerializeField] int damageAmount = 1;

      EnemyStats enemy;

      Collider collider;

      private void Awake()
      {
            enemy = GetComponentInParent<EnemyStats>();
            collider = GetComponent<MeshCollider>();
      }

      private void Update()
      {
            if (enemy.isDead)
            {
                  collider.enabled = false;
            }
      }

      private void OnTriggerEnter(Collider other)
      {
            if(other.gameObject.tag == "Player")
            {
                  other.GetComponent<IHittable>().GetHit(damageAmount);
                  Debug.Log("Hit Player");
            }
      }
}
