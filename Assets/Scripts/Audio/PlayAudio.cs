using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    public AudioClip backGroundMusic;
    public AudioSource sword;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlayBGM(backGroundMusic, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
