using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetTestLevel : MonoBehaviour
{

      private void OnTriggerEnter(Collider other)
      {
            if (other.tag == "Player")
            {
                  //Debug.Log("Loading..."); 
                  SceneManager.LoadScene(2);
            }
      }
}
