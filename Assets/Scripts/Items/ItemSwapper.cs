using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;

public class ItemSwapper : MonoBehaviour
{
      private Hotbar hotbar;
      [SerializeField] private UseItem itemToApply;

      private void OnTriggerEnter(Collider other)
      {
            if(other.gameObject.tag == "Player")
            {
                  hotbar = other.GetComponentInChildren<Hotbar>();
                  Debug.Log($"ItemSwapper Active + {hotbar}");

                  hotbar.useItem = itemToApply;
                  Destroy(gameObject);
            }
      }
}
