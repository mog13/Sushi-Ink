using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class SFXController : MonoBehaviour
    {
        public static SFXController Instance;
        
        public AudioClip placeClip;
        public AudioClip pickClip;
        public AudioClip ClickClip;
        private AudioSource _audio;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
                Instance = this;
            }
        }

        public void PlayClick()
        {
            _audio.PlayOneShot(ClickClip);
        }
        public void PlayPickUp()
        {
            _audio.PlayOneShot(pickClip);
        }
        
        public void PlayPutDown()
        {
            _audio.PlayOneShot(placeClip);
        }
        private void Start()
        {
            _audio = GetComponent<AudioSource>();
        }
    }
}