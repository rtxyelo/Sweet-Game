using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VictoryController : MonoBehaviour
{
    private const int _levelsCount = 10;

    private List<int> _chestCountByLevel = new(_levelsCount) { 2, 2, 2, 2, 1, 3, 3, 3, 4, 5 };

    private List<int> _bombCountByLevel = new(_levelsCount) { 2, 2, 3, 4, 4, 4, 5, 6, 7, 8 };

    private readonly string _levelKey = "Level";

    private readonly string _chestCountKey = "ChestCount";

    private readonly string _bombCountKey = "BombCount";

    private int _currentLevel;

    private int _chestCount;

    private int _bombCount;

    [SerializeField]
    private TMP_Text _chestCounter;

    [SerializeField]
    private TMP_Text _bombCounter;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(_levelKey))
            PlayerPrefs.SetInt(_levelKey, 0);

        if (!PlayerPrefs.HasKey(_chestCountKey))
            PlayerPrefs.SetInt(_chestCountKey, 0);

        if (!PlayerPrefs.HasKey(_bombCountKey))
            PlayerPrefs.SetInt(_bombCountKey, 0);

        _currentLevel = PlayerPrefs.GetInt(_levelKey, 0);

    }

    private void OnEnable()
    {
        GeneratePrize();
        Debug.Log("ENABLE GENERATE PRIZE!====");
    }

    private void GeneratePrize()
    {
        _bombCount = Random.Range(1, _bombCountByLevel[_currentLevel]);
        _chestCount = Random.Range(1, _chestCountByLevel[_currentLevel]);

        _chestCounter.text = _chestCount.ToString();
        _bombCounter.text = _bombCount.ToString();
    }

    public void ClaimPrize()
    {
        PlayerPrefs.SetInt(_chestCountKey, PlayerPrefs.GetInt(_chestCountKey, 0) + _chestCount);
        PlayerPrefs.SetInt(_bombCountKey, PlayerPrefs.GetInt(_bombCountKey, 0) + _bombCount);

        SceneController.LoadScene("Menu");
    }
}
