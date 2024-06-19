using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private const int _levelsCount = 10;

    private readonly string _levelKey = "Level";

    private readonly string _maxLevelKey = "MaxLevel";

    private readonly string _chestCountKey = "ChestCount";

    private readonly string _bombCountKey = "BombCount";

    private List<int> _cellsCountByLevel = new(_levelsCount) { 9, 9, 9, 9, 12, 12, 12, 12, 12, 12};

    private List<float> _showTimeByLevel = new(_levelsCount) { 5f, 4f, 3f, 2f, 5f, 4.5f, 3.5f, 3f, 2.5f, 2f };

    private List<float> _gameTimeByLevel = new(_levelsCount) { 15f, 14f, 13f, 12f, 11f, 10f, 9f, 8f, 7f, 6f };

    [SerializeField]
    private List<Cell> _cells = new();

    private GenerateGameField _gameFieldGenerator;

    [SerializeField]
    private List<Sprite> _fruitsSprite = new(3);

    [SerializeField]
    private TMP_Text _timeCounterText;

    [SerializeField]
    private TMP_Text _levelCounterText;

    [SerializeField]
    private TMP_Text _bombCounterText;

    [SerializeField]
    private GameObject _losePanel;
    
    [SerializeField]
    private GameObject _winPanel;

    private int _currentLevel;

    private List<int> _readyGameField;

    private float _elapsedTime;

    private bool _isGameOver = false;

    private bool _isGameStart = false;

    private bool _calcShowTime = false;

    private bool _calcGameTime = false;

    private List<int> _selectedCellsIndex = new();

    private List<int> _selectedCellsSpriteIndex = new();

    private List<int> _randomCellsIndex = new();

    private List<int> _randomCellsSpriteIndex = new();

    private int _countOfCellsRepeat = 0;

    private List<int> _openCells = new();

    private int _hidedCells = 0;

    private AudioController _audioController;

    private List<int> _openingCellsIndexed = new();

    private Button _bombButton;

    public List<int> OpeningCellsIndexed { get { return _openingCellsIndexed; } }

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(_chestCountKey))
            PlayerPrefs.SetInt(_chestCountKey, 0);

        if (!PlayerPrefs.HasKey(_bombCountKey))
            PlayerPrefs.SetInt(_bombCountKey, 0);

        //// DEBUG !!
        //PlayerPrefs.SetInt(_levelKey, 0);
        //PlayerPrefs.SetInt(_maxLevelKey, 0);
        //PlayerPrefs.SetInt(_chestCountKey, 0);
        //PlayerPrefs.SetInt(_bombCountKey, 0);

        _currentLevel = PlayerPrefs.GetInt(_levelKey, 0);

        _levelCounterText.text = "Level: " + (_currentLevel + 1).ToString();

        _bombCounterText.text = PlayerPrefs.GetInt(_bombCountKey, 0).ToString();

        _gameFieldGenerator = GetComponent<GenerateGameField>();

        for (int i = 0; i < _cellsCountByLevel[_currentLevel]; i++)
        {
            _cells[i].gameObject.SetActive(true);
            _cells[i].CellHideEvent.AddListener(CalculateCellsHide);
            _cells[i].CellIndexedStateEvent.AddListener(CalculateCellsState);
        }

        _readyGameField = _gameFieldGenerator.GenerateField(_cellsCountByLevel[_currentLevel]);

        for (int i = 0; i < _cellsCountByLevel[_currentLevel]; i++)
        {
            _cells[i].Sprite = _fruitsSprite[_readyGameField[i]];
            _cells[i].SpriteId = _readyGameField[i];
        }

        _audioController = FindObjectOfType<AudioController>();

        _bombButton = GameObject.FindGameObjectWithTag("BombButton").GetComponent<Button>();
    }

    private void Start()
    {
        StartCoroutine(ShowItemsCoroutine());
        _calcShowTime = true;
    }

    private void Update()
    {
        if (!_isGameOver)
        {
            if (_calcShowTime)
            {
                _elapsedTime -= Time.deltaTime;
                _timeCounterText.text = ((int)_elapsedTime).ToString();
                if (_elapsedTime < 0)
                {
                    _calcShowTime = false;
                    _calcGameTime = true;
                    _elapsedTime = _gameTimeByLevel[_currentLevel];
                }
            }
            else if (_calcGameTime && _isGameStart)
            {
                _elapsedTime -= Time.deltaTime;
                _timeCounterText.text = ((int)_elapsedTime).ToString();
                if (_elapsedTime < 0)
                {
                    _isGameStart = false;
                    _calcGameTime = false;
                    _elapsedTime = 0;
                    LoseGame();
                }
            }

            if (_countOfCellsRepeat == _cellsCountByLevel[_currentLevel] / _gameFieldGenerator.FruitRepeatCapacity)
            {
                Debug.Log("Victory!!!");
                WinGame();
            }
            //Debug.Log("_countOfCellsRepeat " + _countOfCellsRepeat);

            if (_openingCellsIndexed.Count > 0)
                _bombButton.interactable = false;
            else
                _bombButton.interactable = true;

            Debug.Log("_openingCellsIndexed.count " + _openingCellsIndexed.Count);
        }
    }

    private void LoseGame()
    {
        Debug.Log("Lose!!!");
        _isGameOver = true;
        _isGameStart = false;

        if (_audioController != null)
            _audioController.PlayLoseSound();

        _losePanel.SetActive(true);
    }

    private void WinGame()
    {
        Debug.Log("Win!!!");
        _isGameOver = true;
        _isGameStart = false;

        _winPanel.SetActive(true);

        if (_audioController != null)
            _audioController.PlayWinSound();

        if (PlayerPrefs.GetInt(_levelKey) == PlayerPrefs.GetInt(_maxLevelKey) && PlayerPrefs.GetInt(_maxLevelKey) < _levelsCount)
            PlayerPrefs.SetInt(_maxLevelKey, PlayerPrefs.GetInt(_maxLevelKey) + 1);
    }

    private void CalculateCellsHide()
    {
        _hidedCells++;
        if (_hidedCells == _cellsCountByLevel[_currentLevel])
        {
            _isGameStart = true;
        }
    }

    private void CalculateCellsState(int buttonIndex, int spriteId)
    {
        _openCells.Add(buttonIndex);

        if (_selectedCellsSpriteIndex.Count == 0 || _selectedCellsSpriteIndex[^1] == spriteId || _randomCellsSpriteIndex.Contains(spriteId) && _selectedCellsSpriteIndex[^1] == spriteId)
        {
            _selectedCellsIndex.Add(buttonIndex);
            _selectedCellsSpriteIndex.Add(spriteId);


            int countOfSameSpriteIdRandomOpened = _randomCellsSpriteIndex.FindAll(x => x == spriteId).Count;
            int countOfSameSpriteIdSelectOpened = _selectedCellsSpriteIndex.FindAll(x => x == spriteId).Count;

            Debug.Log("countOfSameSpriteIdRandomOpened " + countOfSameSpriteIdRandomOpened);
            Debug.Log("countOfSameSpriteIdSelectOpened" + countOfSameSpriteIdSelectOpened);


            if (_selectedCellsSpriteIndex.Count == _gameFieldGenerator.FruitRepeatCapacity && countOfSameSpriteIdRandomOpened == 0)
            {
                _countOfCellsRepeat++;

                if (_audioController != null)
                    _audioController.PlayGoodSound();

                _selectedCellsIndex.Clear();
                _selectedCellsSpriteIndex.Clear();
            }
            else if (countOfSameSpriteIdRandomOpened + countOfSameSpriteIdSelectOpened == _gameFieldGenerator.FruitRepeatCapacity)
            {
                _countOfCellsRepeat++;

                if (_audioController != null)
                    _audioController.PlayGoodSound();

                _selectedCellsIndex.Clear();
                _selectedCellsSpriteIndex.Clear();

                _randomCellsIndex.RemoveAll(item => item == buttonIndex);
                _randomCellsSpriteIndex.RemoveAll(item => item == _readyGameField[buttonIndex]);
            }
        }
        else
        {
            if (_openCells.Contains(buttonIndex))
            {
                _openCells.Remove(buttonIndex);
                _cells[buttonIndex].HideCell();
            }

            foreach (var btnInd in _selectedCellsIndex)
            {
                if (_openCells.Contains(btnInd))
                {
                    _openCells.Remove(btnInd);
                    _cells[btnInd].HideCell();
                }
            }

            _selectedCellsIndex.Clear();
            _selectedCellsSpriteIndex.Clear();
        }
    }

    public void ShowRandomFruit()
    {
        if (_isGameStart && PlayerPrefs.GetInt(_bombCountKey) > 0)
        {
            int buttonIndex = Random.Range(0, _cellsCountByLevel[_currentLevel]);

            while (_openCells.Contains(buttonIndex))
                buttonIndex = Random.Range(0, _cellsCountByLevel[_currentLevel]);



            _openCells.Add(buttonIndex);
            _randomCellsIndex.Add(buttonIndex);
            _randomCellsSpriteIndex.Add(_readyGameField[buttonIndex]);

            PlayerPrefs.SetInt(_bombCountKey, PlayerPrefs.GetInt(_bombCountKey, 0) - 1);
            _bombCounterText.text = PlayerPrefs.GetInt(_bombCountKey, 0).ToString();

            int countOfSameSpriteIdRandomOpened = _randomCellsSpriteIndex.FindAll(x => x == _readyGameField[buttonIndex]).Count;
            int countOfSameSpriteIdSelectOpened = _selectedCellsSpriteIndex.FindAll(x => x == _readyGameField[buttonIndex]).Count;

            if (_randomCellsSpriteIndex.FindAll(x => x == _readyGameField[buttonIndex]).Count == _gameFieldGenerator.FruitRepeatCapacity)
            {
                _countOfCellsRepeat++;

                _randomCellsIndex.RemoveAll(item => item == buttonIndex);
                _randomCellsSpriteIndex.RemoveAll(item => item == _readyGameField[buttonIndex]);
            }
            else if (countOfSameSpriteIdRandomOpened + countOfSameSpriteIdSelectOpened == _gameFieldGenerator.FruitRepeatCapacity)
            {
                _countOfCellsRepeat++;
                _selectedCellsIndex.Clear();
                _selectedCellsSpriteIndex.Clear();

                _randomCellsIndex.RemoveAll(item => item == buttonIndex);
                _randomCellsSpriteIndex.RemoveAll(item => item == _readyGameField[buttonIndex]);
            }

            _cells[buttonIndex].ShowCellRandom(buttonIndex);
        }
    }

    private IEnumerator ShowItemsCoroutine()
    {
        _elapsedTime = _showTimeByLevel[_currentLevel];
        yield return new WaitForSeconds(_showTimeByLevel[_currentLevel]);
        HideItems();
    }

    private void HideItems()
    {
        for (int i = 0; i < _cellsCountByLevel[_currentLevel]; i++)
            _cells[i].HideCell();
    }
}
