using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballSpell : Item
{
      public GameObject fireballPrefab;
      public Transform user;
      public float speed;
      public int maxHits = 2;
      public float cooldown = 8f;
      public float manaCost = 50f;
      public float radius = 5f;
      private float lastCastTime;
      private GameObject fireball;

      public override void Use()
      {
            if (Time.time > lastCastTime + cooldown)
            {
                  if (itemLevel == 0)
                  {
                        fireball = Instantiate(fireballPrefab, user.transform.position + user.transform.forward, user.transform.rotation);
                        fireball.GetComponent<FireballSpell>().maxHits = maxHits;
                        fireball.GetComponent<FireballSpell>().manaCost = manaCost;
                        lastCastTime = Time.time;
                  }
                  else if (itemLevel == 1)
                  {
                        fireball = Instantiate(fireballPrefab, user.transform.position + user.transform.forward, user.transform.rotation);
                        fireball.GetComponent<FireballSpell>().maxHits = maxHits;
                        fireball.GetComponent<FireballSpell>().manaCost = manaCost * 0.75f;
                        lastCastTime = Time.time;
                  }
                  else if (itemLevel == 2)
                  {
                        for (int i = 0; i < 3; i++)
                        {
                              fireball = Instantiate(fireballPrefab, user.transform.position + user.transform.forward, Quaternion.Euler(0, user.transform.eulerAngles.y - 15 + (i * 15), 0));
                              fireball.GetComponent<FireballSpell>().maxHits = maxHits;
                              fireball.GetComponent<FireballSpell>().manaCost = manaCost * 0.75f;
                        }
                        lastCastTime = Time.time;
                  }
                  else if (itemLevel == 3)
                  {
                        fireball = Instantiate(fireballPrefab, user.transform.position + user.transform.forward, user.transform.rotation);
                        fireball.GetComponent<FireballSpell>().maxHits = maxHits;
                        fireball.GetComponent<FireballSpell>().manaCost = manaCost * 0.75f;
                  }
            }
      }
}