using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(AudioSource))]
public class Resources : MonoBehaviour
{
      [field: SerializeField] public ResourceDataSO ResourceData { get; set; }

      //AudioSource audioSource;

      private void Awake()
      {
            //audioSource = GetComponent<AudioSource>();
      }

      public void PickupResource()
      {
            StartCoroutine(DestroyCoroutine());
      }

      IEnumerator DestroyCoroutine()
      {
            GetComponent<BoxCollider>().enabled = false;
            //audioSource.Play();
            yield return null;
            Destroy(gameObject);

      }
}
