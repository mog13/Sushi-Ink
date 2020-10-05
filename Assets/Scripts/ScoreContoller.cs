using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;

public class ScoreContoller : MonoBehaviour
{
    public static ScoreContoller Instance;

    public string prefix = "¥";
    public float score;
    public float timeLeft = 600;
    private List<Recipe> _successfullSales = new List<Recipe>();
    private int _unhappyCustomers = 0;
    private TextMeshProUGUI _scoreText;
    private TextMeshProUGUI _timeText;
    private bool levelEnded = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        var texts = GetComponentsInChildren<TextMeshProUGUI>();
        _scoreText = texts[0];
        _timeText = texts[1];
        SetScoreText();
        if (TransitionController.Instance.selectedLevel.sceneName != null)
        {
            timeLeft = TransitionController.Instance.selectedLevel.timeToComplete;
        }
    }

    private void SetScoreText()
    {
        _scoreText.text = prefix + score.ToString("F2");
    }

    private void SetTimeText()
    {
        float mins = Mathf.Floor(timeLeft / 60);
        float secs = Mathf.Round(timeLeft % 60);
        _timeText.text = "0" + mins + "-" + (secs < 10 ? ("0" + secs) : secs.ToString());
    }

    private void Update()
    {
        timeLeft -= Time.deltaTime;
        SetTimeText();
        if (!levelEnded && timeLeft < 0)
        {
            TimeEnded();
        }
    }

    public void UnhappyCustomer()
    {
     if(!levelEnded)   _unhappyCustomers++;
    }

    public void SatisfiedCustomer(Recipe recipe, float timing)
    {
        if (!levelEnded)
        {
            float val = recipe.value;
            if (timing < .35)
            {
                val *= .75f;
            }

            score += val;
            _successfullSales.Add(recipe);
            SetScoreText();
        }
    }

    public void TimeEnded()
    {
        levelEnded = true;
        TransitionController.Instance.EndOfLevel(new LevelOutcome(score, _unhappyCustomers,_successfullSales));
    }
}