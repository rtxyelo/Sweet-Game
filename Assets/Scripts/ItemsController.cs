using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ItemsController : MonoBehaviour
{
    private readonly string _chestCountKey = "ChestCount";
    private readonly string _bombCountKey = "BombCount";

    [SerializeField]
    private ChestDropController _chestDropController;

    [SerializeField]
    private TMP_Text _bombCount;

    [SerializeField]
    private TMP_Text _chestCount;

    [SerializeField]
    private GameObject _dropPanel;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(_chestCountKey))
            PlayerPrefs.SetInt(_chestCountKey, 0);

        if (!PlayerPrefs.HasKey(_bombCountKey))
            PlayerPrefs.SetInt(_bombCountKey, 0);

        if (_bombCount != null)
            _bombCount.text = PlayerPrefs.GetInt(_bombCountKey, 0).ToString();

        if (_chestCount != null)
            _chestCount.text = PlayerPrefs.GetInt(_chestCountKey, 0).ToString();
    }

    //private void Start()
    //{
    //    _chestDropController.PrizeClaimed.AddListener(RefreshItemsInfo);
    //}

    public void RefreshItemsInfo()
    {
        if (_bombCount != null)
            _bombCount.text = PlayerPrefs.GetInt(_bombCountKey, 0).ToString();

        if (_chestCount != null)
            _chestCount.text = PlayerPrefs.GetInt(_chestCountKey, 0).ToString();
    }

    public void OpenChest()
    {
        if (PlayerPrefs.GetInt(_chestCountKey, 0) > 0)
        {
            _dropPanel.SetActive(true);
        }
    }
}
