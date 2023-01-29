using UnityEngine;
using System.Collections.Generic;

public class Hookshots : Item
{
      public GameObject hookshotPrefab;
      public Transform user;
      public float distance;
      public float speed;
      public float pullSpeed;
      private GameObject hookshot;
      private bool isPulling;

      public override void Use()
      {
            hookshot = Instantiate(hookshotPrefab, user.transform.position, user.transform.rotation);
      }

      public override void OnLevelUp()
      {
            distance += 5;
            speed += 1;
            pullSpeed += 2;
      }

      private void Update()
      {
            if (hookshot != null)
            {
                  RaycastHit hit;
                  if (Physics.Raycast(hookshot.transform.position, hookshot.transform.forward, out hit, distance))
                  {
                        if (hit.collider.CompareTag("Wall"))
                        {
                              isPulling = true;
                        }
                  }

                  if (!isPulling)
                  {
                        hookshot.transform.position += hookshot.transform.forward * speed * Time.deltaTime;
                  }
            }
      }
}