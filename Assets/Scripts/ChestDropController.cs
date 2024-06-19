using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChestDropController : MonoBehaviour
{
    [SerializeField]
    private GameObject _bombPrefab;

    [SerializeField]
    private TMP_Text _bombCountText;

    [SerializeField]
    private TMP_Text _receiveText;

    [SerializeField]
    private Button _claimButton;

    [HideInInspector]
    public UnityEvent PrizeClaimed;

    [SerializeField]
    private ItemsController _itemsController;

    private int _bombCountMin = 1;
    private int _bombCountMax = 5;
    private int _bombsCount;

    private readonly string _chestCountKey = "ChestCount";
    private readonly string _bombCountKey = "BombCount";

    private void Awake()
    {
        PrizeClaimed = new UnityEvent();

        if (!PlayerPrefs.HasKey(_chestCountKey))
            PlayerPrefs.SetInt(_chestCountKey, 0);

        if (!PlayerPrefs.HasKey(_bombCountKey))
            PlayerPrefs.SetInt(_bombCountKey, 0);
    }

    private int CalculateBombsCount()
    {
        return Random.Range(_bombCountMin, _bombCountMax);
    }

    public void FinishExpandAnimation()
    {
        _bombsCount = CalculateBombsCount();

        _bombCountText.text = "X" + _bombsCount;

        _bombPrefab.SetActive(true);

        _bombCountText.gameObject.SetActive(true);

        _receiveText.gameObject.SetActive(true);

        _claimButton.gameObject.SetActive(true);
    }

    public void ClaimDrop()
    {
        PlayerPrefs.SetInt(_bombCountKey, PlayerPrefs.GetInt(_bombCountKey, 0) + _bombsCount);

        if (PlayerPrefs.GetInt(_chestCountKey, 0) > 0)
            PlayerPrefs.SetInt(_chestCountKey, PlayerPrefs.GetInt(_chestCountKey, 0) - 1);

        _itemsController.RefreshItemsInfo();

        //PrizeClaimed.Invoke();
    }

    private void OnDisable()
    {
        _bombPrefab.SetActive(false);

        _bombCountText.gameObject.SetActive(false);

        _receiveText.gameObject.SetActive(false);

        _claimButton.gameObject.SetActive(false);
    }
}
