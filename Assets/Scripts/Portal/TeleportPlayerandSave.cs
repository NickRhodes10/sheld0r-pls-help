using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportPlayerandSave : MonoBehaviour
{
    [SerializeField]private int SceneInt;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (other.gameObject.GetComponent<GameManager.PlayerManager>() == true)
            {
                other.gameObject.GetComponent<GameManager.PlayerManager>().SavePlayerManager();
                PlayerPrefs.SetString("Save", "true");
                PlayerPrefs.Save();
            }
            SceneManager.LoadScene(SceneInt);
        }
    }
}
