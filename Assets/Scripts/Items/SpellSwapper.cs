using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;

public class SpellSwapper : MonoBehaviour
{
      private Hotbar hotbar;
      [SerializeField] private UseItem itemToApply;

      private void OnTriggerEnter(Collider other)
      {
            if(other.gameObject.tag == "Player")
            {
                  hotbar = other.GetComponentInChildren<Hotbar>();
                  Debug.Log($"SpellSwapper Active + {hotbar}");
                  // Activate UI
                  //if (Input.GetKeyDown(KeyCode.E))
                  //{
                        hotbar.useMagic = itemToApply;
                        Destroy(gameObject);
                  //}
            }
      }
}
