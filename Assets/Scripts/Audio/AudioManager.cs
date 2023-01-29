using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; //Global variable that every object can see and send messages to the audio manager

    public AudioClip backgroundMusic;
    [SerializeField, Range(1, 15)] private int _sfxAmount = 1; //Determines the amount of SFX sources we can use
    [SerializeField] private GameObject _audioObject;
    [SerializeField, Range(0, 1)] public float backgroundVolume = 0.5f;
    [SerializeField, Range(0, 1)] public float soundEffectVolume = 0.25f;

    public AudioSource[] _sfxSources; //Store all of our SFX sources
    public AudioSource _bgm;
    private int _sfxIndex = 0; //Store the current sfx index

    //Is called at start of game (before the start method)
    private void Awake()
    {
        //Check if the audio manager has been set
        if (AudioManager.instance == null)
        {
            //if not, make me the manager
            AudioManager.instance = this;
        }
        //If there is a manager, and it isn't me
        else if (AudioManager.instance != this)
        {
            //destroy me
            Destroy(this);
        }

        //Now that the manager is set up
        //Init all the needed audio sources
        InitSFX();
        _bgm = gameObject.AddComponent<AudioSource>();


    }

    private void Start()
    {
        PlayBGM(backgroundMusic, 1f);

    }

    private void InitSFX()
    {
        //Setting the size of our array
        _sfxSources = new AudioSource[_sfxAmount];

        //Loop through X amount of times based on the _sfxAmount
        for (int i = 0; i < _sfxAmount; i++)
        {
            //Add a new audio source, and put it into to correct index of the array
            _sfxSources[i] = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlaySFX(AudioClip clipToPlay)
    {
        //Tell the current audio source to load X clip
        _sfxSources[_sfxIndex].clip = clipToPlay;
        //Tell the current audio source to play the loaded clip
        _sfxSources[_sfxIndex].Play();
        //Tick up to next current clip
        _sfxIndex++;

        //Check to see if the index goes out of array bounds
        if (_sfxIndex > _sfxSources.Length - 1)
        {
            //if so, reset index to 0
            _sfxIndex = 0;
        }
    }

    public void PlaySFX(AudioClip clipToPlay, float volume)
    {
        _sfxSources[_sfxIndex].clip = clipToPlay;
        _sfxSources[_sfxIndex].volume = volume;
        _sfxSources[_sfxIndex].Play();

        _sfxIndex++;

        //Check to see if the index goes out of array bounds
        if (_sfxIndex > _sfxSources.Length - 1)
        {
            //if so, reset index to 0
            _sfxIndex = 0;
        }
    }

    public void PlaySFX(AudioClip clipToPlay, Vector3 position, float volume, float spatialBlend = 1)
    {
        if (_audioObject != null)
        {
            GameObject go = Instantiate(_audioObject, position, Quaternion.identity);

            if (go.GetComponent<AudioSource>() == null)
            {
                go.AddComponent<AudioSource>();
            }

            AudioSource temp = go.GetComponent<AudioSource>();
            temp.clip = clipToPlay;
            temp.volume = volume;
            temp.spatialBlend = spatialBlend;
            temp.Play();

            StartCoroutine(CleanUp(go, clipToPlay.length));
        }
    }

    public void PlayBGM(AudioClip musicToPlay, float fadeDuration, bool isLooping = true)
    {

        Debug.Log("PLay: " + musicToPlay);
        StartCoroutine(PlayBGMCo(musicToPlay, fadeDuration, isLooping, backgroundVolume));
    }

    private IEnumerator CleanUp(GameObject go, float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(go);
    }

    private IEnumerator PlayBGMCo(AudioClip musicToPlay, float fadeDuration, bool isLooping, float volume)
    {
        Debug.Log("CO: " + musicToPlay);

        AudioSource newBGM = gameObject.AddComponent<AudioSource>();
        newBGM.clip = musicToPlay;
        newBGM.volume = 0;
        newBGM.loop = isLooping;
        newBGM.Play();

        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float percent = t / fadeDuration;
            _bgm.volume = Mathf.Lerp(volume, 0, percent);
            newBGM.volume = Mathf.Lerp(0, volume, percent);
            yield return null;
        }

        Destroy(_bgm);
        _bgm = newBGM;
    }
}