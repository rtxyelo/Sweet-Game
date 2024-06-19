using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField]
    private AudioSource _musicSource;
    
    [SerializeField]
    private AudioSource _goodSound;   
    
    [SerializeField]
    private AudioSource _winSound;    
    
    [SerializeField]
    private AudioSource _loseSound;

    private readonly string _volumeKey = "Volume";

    public static AudioController Instance;

    public float Volume;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //Debug.Log("Instance create! " + Instance);
        
        DontDestroyOnLoad(gameObject);
    }
    private void OnDisable()
    {
        PlayerPrefs.SetFloat(_volumeKey, Volume);
    }
    public void Initialize()
    {
        if (!PlayerPrefs.HasKey(_volumeKey))
            PlayerPrefs.GetFloat(_volumeKey, 0);

        Volume = PlayerPrefs.GetFloat(_volumeKey);
    }
    public void ChangeVolume(float newVolume)
    {
        Volume = newVolume;
        PlayerPrefs.SetFloat(_volumeKey, Volume);
        _musicSource.volume = Volume;
    }

    public void PlayGoodSound()
    {
        if (_goodSound != null)
        {
            _goodSound.volume = Volume;
            _goodSound.Play();
        }
    }

    public void PlayWinSound()
    {
        if (_winSound != null)
        {
            _winSound.volume = Volume;
            _winSound.Play();
        }
    } 
    
    public void PlayLoseSound()
    {
        if (_loseSound != null)
        {
            _loseSound.volume = Volume;
            _loseSound.Play();
        }
    }
}
