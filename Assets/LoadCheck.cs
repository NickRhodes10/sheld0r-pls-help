using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCheck : MonoBehaviour
{
    UnityEngine.UI.Button LoadGame;

    private void Awake()
    {
        PlayerSaving.SaveToken.PlayerSaveToken data = PlayerSaving.StatsSaveLoad.Load();
        if(data == null)
        {
            LoadGame.interactable = false;
        }
        else
        {
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
