using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Xsl;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    private const int _levelsCount = 10;

    [SerializeField]
    private TMP_Text _levelCounterText;

    [SerializeField]
    private GameObject _lockLevelPanel;

    [SerializeField]
    private GameObject _alreadySelectPanel;

    [SerializeField]
    private List<Sprite> _chooseButtonSprites = new(2);

    [SerializeField]
    private List<GameObject> _levelObjects = new(_levelsCount);

    private readonly string _levelKey = "Level";
    private readonly string _maxLevelKey = "MaxLevel";

    private List<GameObject> _lockIcons = new(_levelsCount);
    private List<GameObject> _levelTexts = new(_levelsCount);

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(_levelKey))
            PlayerPrefs.SetInt(_levelKey, 0);

        if (!PlayerPrefs.HasKey(_maxLevelKey))
            PlayerPrefs.SetInt(_maxLevelKey, 0);

        _levelObjects.ForEach(lvl => _levelTexts.Add(lvl.transform.GetChild(0).gameObject));
        _levelObjects.ForEach(lvl => _lockIcons.Add(lvl.transform.GetChild(1).gameObject));

        Debug.Log("Level texts count " + _levelTexts.Count);
        Debug.Log("Lock icons count " + _lockIcons.Count);

    }

    private void Initialize()
    {
        int maxLvl = PlayerPrefs.GetInt(_maxLevelKey, 0);
        int currentLvl = PlayerPrefs.GetInt(_levelKey, 0);
        Debug.Log("Max level " + maxLvl);
        Debug.Log("Current level " + currentLvl);

        for (int i = 0; i < _levelObjects.Count; i++)
        {
            if (i <= maxLvl)
            {
                _lockIcons[i].SetActive(false);
                _levelTexts[i].SetActive(true);
            }
            else
            {
                _lockIcons[i].SetActive(true);
                _levelTexts[i].SetActive(false);
            }
        }

        _levelCounterText.text = "Level: " + (currentLvl + 1).ToString();

        ChangeLevelSprites(currentLvl);
    }

    private void ChangeLevelSprites(int currentLvl)
    {
        for (int i = 0; i < _levelObjects.Count; i++)
        {
            if (i == currentLvl)
                _levelObjects[i].GetComponent<Image>().sprite = _chooseButtonSprites[0];
            else
                _levelObjects[i].GetComponent<Image>().sprite = _chooseButtonSprites[1];
        }
    }

    private void OnEnable()
    {
        Initialize();
    }

    public void SelectLevel(int lvl)
    {
        int currentLvl = PlayerPrefs.GetInt(_levelKey, 0);
        int maxLvl = PlayerPrefs.GetInt(_maxLevelKey, 0);

        if (lvl <= maxLvl && lvl != currentLvl)
        {
            PlayerPrefs.SetInt(_levelKey, lvl);
            currentLvl = PlayerPrefs.GetInt(_levelKey, 0); 
            ChangeLevelSprites(currentLvl);
        }
        else if (lvl == currentLvl)
        {
            _alreadySelectPanel.SetActive(true);
        }
        else
        {
            _lockLevelPanel.SetActive(true);
        }

        _levelCounterText.text = "Level: " + (currentLvl + 1).ToString();
    }
}
