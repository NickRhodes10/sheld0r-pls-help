using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
      [SerializeField] private List<GameObject> enemyPrefabs;                                               // Prefabs to instantiate
      [SerializeField] private List<GameObject> spawnPoints;                                                // Hold spawnpoints
      [SerializeField] private int count = 20;                                                              // Number of enemies to spawn
      [SerializeField] private float minDelay = 0.8f, maxDelay = 1.5f;                                      // Delay between enemies spawning

      IEnumerator SpawnCoroutine()
      {
            while(count > 0)
            {
                  count--;
                  var randomIndex = Random.Range(0, spawnPoints.Count);                                     // Determine a random index within the spawnPoints list

                  var randomOffset = Random.insideUnitCircle;                                               // Select a random offset within unit circle, just a nearby point
                  var spawnPoint = spawnPoints[randomIndex].transform.position + (Vector3)randomOffset;     // Add offset to spawnPoint

                  SpawnEnemy(spawnPoint);                                                                   // Spawn enemy @ spawnPoint

                  var randomTime = Random.Range(minDelay, maxDelay);                                        // Determine random amount of time to wait between enemy spawns, value between min and max delay thresholds
                  yield return new WaitForSeconds(randomTime);                                              // Wait until our randomTime is over
            }
      }

      private void SpawnEnemy(Vector3 spawnPoint)
      {
            Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], spawnPoint, Quaternion.identity);  // Spawn enemy @ spawn point
      }

      private void Update()
      {
            // Can Refill Count here for infinitely spawning enemies
            // if(count == 0)
            // {
            //    count = #;
            // }
      }

      private void Start()
      {
            if(spawnPoints.Count > 0)                                                                       // If we have spawnPoints,
            {
                  foreach ( var spawnPoint in spawnPoints)                                                  // Loop through each spawnPoint
                  {
                        SpawnEnemy(spawnPoint.transform.position);                                          // Spawn enemy at each spawnPoint
                  }
            }

            StartCoroutine(SpawnCoroutine());                                                               // Restart the loop
      }
}
