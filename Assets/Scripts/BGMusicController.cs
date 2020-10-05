using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusicController : MonoBehaviour
{
    public AudioClip[] tracks;
    private AudioSource _audio;
    public static BGMusicController Instance;
    private int _playingTrack = 0;
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void PlayTrack(int n)
    {
        if (n!=_playingTrack && n < tracks.Length)
        {
            _audio.clip = tracks[n];
            _playingTrack = n;
            _audio.Play();
        }
    }
}
