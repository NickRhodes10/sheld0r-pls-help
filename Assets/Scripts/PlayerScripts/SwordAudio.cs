using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAudio : MonoBehaviour
{
    public AudioClip swishOne;
    public AudioClip swishTwo;
    public AudioClip swishThree;
    public AudioManager _am;

    private void Start()
    {
        //_am = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        _am = AudioManager.instance;
    }

    public void PlaySwishOne()
    {
        if (_am != null)
            _am.PlaySFX(swishOne, 0.5f);
    }
    public void PlaySwishTwo()
    {
        if (_am != null)
            _am.PlaySFX(swishTwo, 0.5f);
    }
    public void PlaySwishThree()
    {
        if (_am != null)
            _am.PlaySFX(swishThree, 0.5f);
    }
}
