using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAudio : MonoBehaviour
{
    //public AudioClip bgm;
    public AudioClip torchSFX;

    private AudioManager _am;

    // Start is called before the first frame update
    void Start()
    {
        _am = AudioManager.instance;

        //_am.PlayBGM(bgm, 0f);
        _am.PlaySFX(torchSFX);
    }
}
