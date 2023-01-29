using UnityEngine;
using System.Collections.Generic;

public class Lightning : Item
{
      public GameObject lightningPrefab;
      public Transform user;
      public float initialRadius;
      public int damage;
      public float manaCost = 50f;
      public float aoeCooldown = 20f;
      public float aoeRadius;
      private float lastAoeCastTime;
      private bool isAoeActive = false;

      public override void Use()
      {
            if (itemLevel == 0)
            {
                  GameObject lightning = Instantiate(lightningPrefab, user.transform.position, user.transform.rotation);
                  lightning.GetComponent<Lightning>().damage = damage;
                  lightning.GetComponent<Lightning>().initialRadius = initialRadius;
                  lightning.GetComponent<Lightning>().manaCost = manaCost;
            }
            else if (itemLevel == 1)
            {
                  GameObject lightning = Instantiate(lightningPrefab, user.transform.position, user.transform.rotation);
                  lightning.GetComponent<Lightning>().damage = damage;
                  lightning.GetComponent<Lightning>().initialRadius = initialRadius;
                  lightning.GetComponent<Lightning>().manaCost = manaCost * 0.75f;
            }
      }

      public void OnHit()
      {
            if (itemLevel == 3 && Time.time > lastAoeCastTime + aoeCooldown)
            {
                  List<Collider> enemies = new List<Collider>(Physics.OverlapSphere(user.transform.position, aoeRadius, LayerMask.GetMask("Enemy")));
                  if (enemies.Count > 0)
                  {
                        foreach (Collider enemy in enemies)
                        {
                              enemy.GetComponent<EnemyStats>().GetHit(damage);
                              enemy.GetComponent<EnemyStats>().GetStunned(1f);
                        }
                        lastAoeCastTime = Time.time;
                  }
            }
      }
}


