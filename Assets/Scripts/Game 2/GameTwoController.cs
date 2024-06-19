using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameTwoController : BaseGameController
{
    private const int _levelsCount = 5;

    [SerializeField]
    private FruitSpawner _fruitSpawner;

    public override int GameTime { get => (int)_gameSessionTime; }

    private float _gameSessionTime = 180f;

    [HideInInspector]
    public UnityEvent IsGameOver;

    private ScoreCounter _scoreCounter;

    [SerializeField]
    private InputController _inputController;

    [Space]
    [Header("Game Over Panels")]
    [SerializeField]
    private GameObject _winPanel;

    [SerializeField]
    private GameObject _losePanel;

    private readonly string _levelKey = "LevelGameTwo";

    private readonly string _maxLevelKey = "MaxLevelGameTwo";

    private int _currentLevel;

    private int _maxLevel;

    private List<float> _spawnTimeByLevel = new(5) { 1.5f, 1.3f, 1.0f, 0.8f, 0.5f };

    private List<int> _levelScoreList = new(4) { 100, 130, 175, 220, 300 };

    private bool _isGameOver = false;

    private int _finalScore = 0;

    private int _currentLevelScore;

    private AudioController _audioController;

    public override int CurrentLevelScore { get { return _currentLevelScore; } }

    public void Awake()
    {
        if (!PlayerPrefs.HasKey(_levelKey))
            PlayerPrefs.SetInt(_levelKey, 0);

        if (!PlayerPrefs.HasKey(_maxLevelKey))
            PlayerPrefs.SetInt(_maxLevelKey, 0);

        _maxLevel = PlayerPrefs.GetInt(_maxLevelKey);

        _currentLevel = PlayerPrefs.GetInt(_levelKey);

        IsGameOver = new UnityEvent();

        _scoreCounter = FindObjectOfType<ScoreCounter>();

        _currentLevelScore = _levelScoreList[_currentLevel];

        _audioController = FindObjectOfType<AudioController>();
    }

    private void Start()
    {
        _fruitSpawner.SpawnTime = _spawnTimeByLevel[_currentLevel];
    }

    private void GameWin()
    {
        Debug.Log("Game win!");
        _isGameOver = true;

        if (_scoreCounter != null)
            _finalScore = _scoreCounter.Score;

        Debug.Log("Final score " + _finalScore);

        if (_currentLevel != _levelScoreList.Count)
        {
            if (_currentLevel == _maxLevel && _finalScore >= _currentLevelScore)
            {
                _maxLevel++;
                PlayerPrefs.SetInt(_maxLevelKey, _maxLevel);
                Debug.Log("MAX LVL " + PlayerPrefs.GetInt(_maxLevelKey));
            }
        }

        IsGameOver.Invoke();

        _inputController.IsActive = false;

        if (_audioController != null)
            _audioController.PlayWinSound();

        _winPanel.SetActive(true);
    }

    private void GameLose()
    {
        Debug.Log("Game lose!");
        _isGameOver = true;

        if (_scoreCounter != null)
            _finalScore = _scoreCounter.Score;

        Debug.Log("Final score" + _finalScore);

        IsGameOver.Invoke();

        _inputController.IsActive = false;

        if (_audioController != null)
            _audioController.PlayLoseSound();

        _losePanel.SetActive(true);
    }

    private void Update()
    {
        _gameSessionTime -= Time.deltaTime;

        _finalScore = _scoreCounter.Score;

        if (_gameSessionTime < 0 && !_isGameOver && _finalScore < _currentLevelScore)
        {
            GameLose();
        }
        else if (_finalScore >= _currentLevelScore && !_isGameOver)
        {
            GameWin();
        }
    }
}
