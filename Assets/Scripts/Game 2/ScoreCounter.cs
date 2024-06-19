using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _scoreText;

    private int _score = 0;

    private BaseGameController _gameController;

    public int Score { get => _score; set => _score = value; }

    private readonly string _recordKey = "Record";

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(_recordKey))
            PlayerPrefs.SetInt(_recordKey, 0);

        _gameController = FindObjectOfType<BaseGameController>();
    }

    private void Start()
    {
        _scoreText.text = _score.ToString() + "/" + _gameController.CurrentLevelScore;
    }

    public void IncreaseScore()
    {
        //Debug.Log("Score increase!");

        _score++;

        _scoreText.text = _score.ToString() + "/" + _gameController.CurrentLevelScore;

        if (_score > PlayerPrefs.GetInt(_recordKey, 0))
            PlayerPrefs.SetInt(_recordKey, _score);
    }
}