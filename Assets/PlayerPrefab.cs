using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefab : MonoBehaviour
{
    public static PlayerPrefab instance;

    private void Awake()
    {
        if (PlayerPrefab.instance == null)
            PlayerPrefab.instance = this;
        else if (PlayerPrefab.instance != this)
            Destroy(this);
    }
}
