using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;

public class ResourceDropper : MonoBehaviour
{
      [SerializeField] private List<ItemSpawnData> itemsToDrop = new List<ItemSpawnData>();
      float[] itemWeights;

      [SerializeField] [Range(0, 1)] private float dropChance = 0.5f;

      private void Start()
      {
            itemWeights = itemsToDrop.Select(item => item.rate).ToArray();
      }

      public void DropItem()
      {
            var dropValue = Random.value;
            if(dropValue < dropChance)
            {
                        int index = GetRandomWeightedIndex(itemWeights);
                        Instantiate(itemsToDrop[index].itemPrefab, transform.position, Quaternion.identity);
                        Debug.Log("Enemy Dropped Item");
            }
      }

      private int GetRandomWeightedIndex(float[] itemWeights)
      {
            float sum = 0f;
            for (int i = 0; i < itemWeights.Length; i++)
            {
                  sum += itemWeights[i];
            }

            float randomValue = Random.Range(0, sum);
            float tempSum = 0f;

            for (int i = 0; i < itemsToDrop.Count; i++)
            {
                  if(randomValue >= tempSum && randomValue < tempSum + itemWeights[i])
                  {
                        return i;
                  }

                  tempSum += itemWeights[i];
            }

            return 0;
      }
}

[Serializable]
public struct ItemSpawnData
{
      [Range(0, 1)] public float rate;
      public GameObject itemPrefab;
}