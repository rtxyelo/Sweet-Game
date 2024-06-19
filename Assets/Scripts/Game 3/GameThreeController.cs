using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameThreeController : BaseGameController
{
    [SerializeField]
    private List<FruitController> _fruits = new();

    [SerializeField]
    private List<Sprite> _fruitTypes = new();

    [Space]
    [Header("Game Over Panels")]
    [SerializeField]
    private GameObject _winPanel;

    [SerializeField]
    private GameObject _losePanel;

    private bool _isGameOver = false;

    private List<float> _spawnTime = new() { 2.5f, 2.3f, 2.0f, 1.5f, 1.2f };

    private List<int> _levelScoreList = new(4) { 50, 65, 75, 100, 120 };

    private List<int> _cellsCountByLevel = new(_levelsCount) { 9, 9, 12, 12, 12 };

    private readonly string _maxLevelKey = "MaxLevelGameThree";

    private readonly string _levelKey = "LevelGameThree";

    private const int _levelsCount = 5;

    private int _maxLevel;

    private int _currentLevel;

    private int _finalScore = 0;

    private float _gameSessionTime = 180f;

    private ScoreCounter _scoreCounter;

    [HideInInspector]
    public UnityEvent IsGameOver;

    private int _currentLevelScore;

    private AudioController _audioController;

    public override int GameTime { get => (int)_gameSessionTime; }

    public override int CurrentLevelScore { get { return _currentLevelScore; } }

    private void Start()
    {
        StartCoroutine(SpawnFruitCoroutine());

        foreach (var fruit in _fruits)
            fruit.FruitIsCatch.AddListener(FruitIsCatch);
    }

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(_levelKey))
            PlayerPrefs.SetInt(_levelKey, 0);

        if (!PlayerPrefs.HasKey(_maxLevelKey))
            PlayerPrefs.SetInt(_maxLevelKey, 0);

        //// DEBUG !!
        //PlayerPrefs.SetInt(_levelKey, 3);
        //PlayerPrefs.SetInt(_maxLevelKey, 4);

        _maxLevel = PlayerPrefs.GetInt(_maxLevelKey);

        _currentLevel = PlayerPrefs.GetInt(_levelKey);

        IsGameOver = new UnityEvent();

        _scoreCounter = FindObjectOfType<ScoreCounter>();

        _currentLevelScore = _levelScoreList[_currentLevel];

        //for (int i = 0; i < _cellsCountByLevel[_currentLevel]; i++)
        //{
        //    _fruits[i].FruitIsCatch.AddListener(FruitIsCatch);
        //}

        for (int i = _fruits.Count - 1; i >= _cellsCountByLevel[_currentLevel]; i--)
        {
            _fruits[i].transform.parent.gameObject.SetActive(false);
        }

        _audioController = FindObjectOfType<AudioController>();
    }

    private IEnumerator SpawnFruitCoroutine()
    {
        while (!_isGameOver)
        {

            int fruitType = Random.Range(0, _fruitTypes.Count);
            int fruitIndex = Random.Range(0, _cellsCountByLevel[_currentLevel]);
            //int fruitIndex = 0;
            
            while (_fruits[fruitIndex].IsActive)
                fruitIndex = Random.Range(0, _cellsCountByLevel[_currentLevel]);

            _fruits[fruitIndex].ActiveFruit(_fruitTypes[fruitType]);

            //yield return new WaitForSeconds(Random.Range(_spawnTime[_currentLevel] - 0.5f, _spawnTime[_currentLevel]));
            yield return new WaitForSeconds(_spawnTime[_currentLevel]);
        }
    }

    private void FruitIsCatch()
    {
        Debug.Log("Fruit is catch!");
        if (_audioController != null)
            _audioController.PlayGoodSound();
        _scoreCounter.IncreaseScore();
    }

    private void GameWin()
    {
        Debug.Log("Game win!");
        _isGameOver = true;

        if (_scoreCounter != null)
            _finalScore = _scoreCounter.Score;

        Debug.Log("Final score" + _finalScore);

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

        Debug.Log("Final score " + _finalScore);

        IsGameOver.Invoke();

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
