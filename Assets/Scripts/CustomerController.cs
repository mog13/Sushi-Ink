using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CustomerController : MonoBehaviour
{
    public Recipe order;
    public float patience = 30;
    public Color fullCol = Color.green;
    public Color emptyCol = Color.red;
    private float _currentPatience;
    private Slider _slider;
    private Image _image;
    private bool _leaving = false;
    private SpriteRenderer _sr;
    [Header("audio clips")] public List<AudioClip> customerHappy;
     public List<AudioClip> customerAngry;
     public AudioClip bell;

    private AudioSource _audioSource;
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _slider = GetComponentInChildren<Slider>();
        _image = _slider.GetComponentInChildren<Image>();
        _sr = GetComponentInChildren<SpriteRenderer>();
        GetComponentsInChildren<Image>()[1].sprite = order.outPut;
        _currentPatience = patience;
        
        if(bell) _audioSource.PlayOneShot(bell);
    }


    void HandleTimer()
    {
        float fraction = _currentPatience / patience;
        _slider.value = fraction;
        _image.color = Color.Lerp(emptyCol, fullCol, fraction);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlateController plate = other.GetComponentInChildren<PlateController>();
        if (!_leaving && plate)
        {
            if (plate.currentRecipe == order)
            {
                Leave();
                plate.Eaten();
                ScoreContoller.Instance.SatisfiedCustomer(order, _currentPatience / patience);
                PlayRandomHappy();
               
            }
        }
    }

    void Leave()
    {
        _leaving = true;
        GetComponentInChildren<Canvas>().enabled = false;
    }
    void PlayRandomHappy()
    {
        AudioClip happyClip = customerHappy[Random.Range(0, customerHappy.Count - 1)];
        _audioSource.PlayOneShot(happyClip);
    }
    
    void PlayRandomAngry()
    {
        AudioClip angryClip = customerAngry[Random.Range(0, customerAngry.Count - 1)];
        _audioSource.PlayOneShot(angryClip);
    }
    private void FixedUpdate()
    {
        if (!_leaving)
        {
            _currentPatience -= Time.deltaTime;
            HandleTimer();
            if (_currentPatience <= 0)
            {
                ScoreContoller.Instance.UnhappyCustomer();
                PlayRandomAngry();
                Leave();
            }
        }
        else
        {
            _sr.color = _sr.color * .9f;
            if (_sr.color.a < 0.05f)
            {
                Destroy(gameObject);
            }
        }
    }
}
