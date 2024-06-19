using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectLevelBehaviour : MonoBehaviour
{
    [SerializeField]
    private Sprite _selectSprite;

    [SerializeField]
    private List<Image> _buttonsImg;

    [SerializeField]
    private Sprite _unselectSprite;

    [SerializeField]
    private List<GameObject> _gameOneItems;

    private int _selectedLevel;

    private void Awake()
    {
        SelectGame(0);
    }

    public void SelectGame(int gameInd)
    {
        _selectedLevel = gameInd;

        for (int i = 0; i < _buttonsImg.Count; i++)
        {
            if (i == _selectedLevel)
                _buttonsImg[i].sprite = _selectSprite;
            else
                _buttonsImg[i].sprite = _unselectSprite;
        }

        bool activeStatus = gameInd == 0;

        foreach (var item in _gameOneItems)
        {
            item.SetActive(activeStatus);
        }
    }

    public void LoadGame()
    {
        switch (_selectedLevel)
        {
            case 0:
                SceneController.LoadScene("Levels");
                break;
            case 1:
                SceneController.LoadScene("Levels 2");
                break;
            case 2:
                SceneController.LoadScene("Levels 3");
                break;
        }
    }
}
