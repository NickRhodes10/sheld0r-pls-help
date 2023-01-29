using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangScript : MonoBehaviour
{
      bool isThrown;                                              // Has the player thrown the boomerang?
      public GameObject player;                                   // Reference to player
      public GameObject BoomerangPrefab;                          // Reference to Boomerang
      public float boomerangTimer = 1.5f;                         // How long the boomerang remains in air
      public float travelDistance = 10f;                          // How far the boomerang travels
      public float throwSpeed = 40f;                              // How quickly the boomerang moves
      public float rotationSpeed;                                 // How fast boomerang spins in air
      Transform itemToRotate;                                     // Reference to boomerang transform
      Vector3 locationInFrontOfPlayer;                            // Where boomerang will travel to


      private void Start()
      {
            isThrown = false;
            player = GameObject.FindGameObjectWithTag("Player");  // Find player

            itemToRotate = gameObject.transform.GetChild(0);      // Find boomerang model
            locationInFrontOfPlayer = new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z) + player.transform.forward * travelDistance;

            StartCoroutine(ThrowBoomerang());
      }

      private void Update()
      {
            itemToRotate.transform.Rotate(0, Time.deltaTime * rotationSpeed, 0);

            if (isThrown)
            {
                  transform.position = Vector3.MoveTowards(transform.position, locationInFrontOfPlayer, Time.deltaTime * throwSpeed);
            }

            if (!isThrown)
            {
                  transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z), Time.deltaTime * throwSpeed);
            }

            if(!isThrown && Vector3.Distance(player.transform.position, transform.position) < 1.5f)
            {
                  Destroy(this.gameObject);
            }
      }

      IEnumerator ThrowBoomerang()
      {
            isThrown = true;
            yield return new WaitForSeconds(boomerangTimer);
            isThrown = false;
      }
}
