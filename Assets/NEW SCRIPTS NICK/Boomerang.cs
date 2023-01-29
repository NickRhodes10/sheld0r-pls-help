using System.Collections.Generic;
using UnityEngine;

public class Boomerang : Item
{
      public GameObject boomerangPrefab;
      public Transform user;
      public float distance;
      public float speed;
      public float rotationSpeed;
      public float cooldown = 3f;
      public float staminaCost = 15f;
      public float stunDuration = 1f;
      public int bouncesLeft = 3;
      public int bounceRange = 5;
      private GameObject boomerang;
      private Vector3 startPos;
      private Vector3 endPos;
      private float lastCastTime;

      public override void Use()
      {
            if (Time.time > lastCastTime + cooldown)
            {
                  if (itemLevel == 0)
                  {
                        startPos = user.transform.position;
                        endPos = startPos + user.transform.forward * distance;
                        boomerang = Instantiate(boomerangPrefab, startPos, user.transform.rotation);
                        boomerang.GetComponent<Boomerang>().stunDuration = stunDuration;
                        boomerang.GetComponent<Boomerang>().staminaCost = staminaCost;
                        lastCastTime = Time.time;
                  }
                  else if (itemLevel == 1)
                  {
                        startPos = user.transform.position;
                        endPos = startPos + user.transform.forward * distance;
                        boomerang = Instantiate(boomerangPrefab, startPos, user.transform.rotation);
                        boomerang.GetComponent<Boomerang>().stunDuration = stunDuration * 1.5f;
                        boomerang.GetComponent<Boomerang>().staminaCost = staminaCost;
                        lastCastTime = Time.time;
                  }
                  else if (itemLevel == 2)
                  {
                        startPos = user.transform.position;
                        endPos = startPos + user.transform.forward * distance;
                        boomerang = Instantiate(boomerangPrefab, startPos, user.transform.rotation);
                        boomerang.GetComponent<Boomerang>().stunDuration = stunDuration * 1.5f;
                        boomerang.GetComponent<Boomerang>().staminaCost = staminaCost * 0.5f;
                        lastCastTime = Time.time;
                  }
                  else if (itemLevel == 3)
                  {
                        startPos = user.transform.position;
                        endPos = startPos + user.transform.forward * distance;
                        boomerang = Instantiate(boomerangPrefab, startPos, user.transform.rotation);
                        boomerang.GetComponent<Boomerang>().stunDuration = stunDuration * 1.5f;
                        boomerang.GetComponent<Boomerang>().staminaCost = staminaCost * 0.5f;
                        lastCastTime = Time.time;
                  }
            }

      }

      private void OnTriggerEnter(Collider other)
      {
            if (other.CompareTag("Enemy"))
            {
                  if (itemLevel == 3)
                  {
                        if (bouncesLeft > 0)
                        {
                              other.GetComponent<EnemyStats>().GetStunned(stunDuration);
                              bouncesLeft--;
                              List<Collider> enemies = new List<Collider>(Physics.OverlapSphere(user.transform.position, bounceRange, LayerMask.GetMask("Enemy")));
                              if (enemies.Count > 0)
                              {
                                    int closestEnemy = 0;
                                    float closestDistance = Vector3.Distance(user.transform.position, enemies[0].transform.position);
                                    for (int i = 1; i < enemies.Count; i++)
                                    {
                                          float distance = Vector3.Distance(user.transform.position, enemies[i].transform.position);
                                          if (distance < closestDistance)
                                          {
                                                closestDistance = distance;
                                                closestEnemy = i;
                                          }
                                    }
                                    Vector3 newDirection = (enemies[closestEnemy].transform.position - user.transform.position).normalized;
                                    endPos = enemies[closestEnemy].transform.position;
                                    user.transform.forward = newDirection;
                              }
                              else
                              {
                                    bouncesLeft = 0;
                                    endPos = startPos;
                              }
                        }
                        else
                        {
                              endPos = startPos;
                        }
                  }
            }
      }

}