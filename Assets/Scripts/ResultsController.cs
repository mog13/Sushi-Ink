using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultsController : MonoBehaviour
{
    private Slider _slider;
    public GameObject knifePrefab;
    public GameObject[] starPrefabs;
    public AudioClip[] starNoises;
    private float[] _starScore;
    private bool[] _starHit = {false,false,false};
    private Vector2[] _starPos = new[] {Vector2.zero, Vector2.zero, Vector2.zero};
    private AudioSource _audio;
    private float _score, _curScore;
    private float _ds = .005f;
    private float _timeTillIncr = .1f;
    private float _dt = 0f;
    private float _maxScore;
    public string prefix = "¥";
    private TextMeshProUGUI[] _texts;
    void Start()
    {
        _texts = GetComponentsInChildren<TextMeshProUGUI>();
        _audio = GetComponent<AudioSource>();
        _slider = GetComponentInChildren<Slider>();
        _slider.value = 0;

        _starScore = TransitionController.Instance.selectedLevel.goals;
        _maxScore = _starScore[2] *1.2f; //3 star score * tolerance
       
        for (var i = 0; i < _starPos.Length; i++)
        {
            float yPos = -10 +(_starScore[i] / _maxScore * 20);
            _starPos[i] = new Vector2(9f, yPos);
            Instantiate(starPrefabs[i], _starPos[i], Quaternion.identity);
        }

        _score = TransitionController.Instance.lastLevelOutcome.score;
        _curScore = 0;
        UpdateScore();
        _texts[1].enabled = false;
    }
    private void UpdateBar()
    {
        _slider.value = _curScore / _maxScore;
    }
    void UpdateScore()
    {
       _texts[0].text =  prefix + _curScore.ToString("F2");
    }
    // Update is called once per frame
    void Update()
    {
        _dt += Time.deltaTime;
        if (_dt > _timeTillIncr)
        {
            _timeTillIncr *= 1.03f;
            _ds += .01f;
            _dt = 0;
        }
    }

    private void FixedUpdate()
    {
        if (_curScore < _score)
        {
            if (_score - _curScore < _ds) _curScore = _score;
            else _curScore += _ds;
            UpdateScore();
            UpdateBar();

            for (var i = 0; i < _starHit.Length; i++)
            {
                if (!_starHit[i] && _curScore > _starScore[i])
                {
                    _audio.PlayOneShot(starNoises[i]);
                    Instantiate(knifePrefab, _starPos[i] - new Vector2(4f,0), Quaternion.identity);
                    _starHit[i] = true;
                }
            }
        }
        else
        {
            _audio.loop= false;
            _texts[1].enabled = true;
            if (Input.GetMouseButtonDown(0))
            {
                TransitionController.Instance.GoToScene("MainMenu");
            }
        }
    }

 
}
