using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum CellState
{
    None,
    Hide,
    Show
}

public class Cell : MonoBehaviour
{
    private CellState _cellState = CellState.Show;

    private Image _fruitImage;
    
    private Button _button;

    private Animator _animator;

    private int _spriteId;

    private int _buttonIndex;

    [HideInInspector]
    public UnityEvent CellHideEvent;

    [HideInInspector]
    public UnityEvent<int, int> CellIndexedStateEvent;

    public Sprite Sprite { get { return _fruitImage.sprite; } set { _fruitImage.sprite = value; } }
    public int SpriteId { get { return _spriteId; } set { _spriteId = value; } }
    public Button Button { get { return _button; } }

    public Animator Animator { get { return _animator; } }

    public CellState CellState { get { return _cellState; } }

    private GameController _gameController;

    private void Awake()
    {
        CellHideEvent = new UnityEvent();
        CellIndexedStateEvent = new UnityEvent<int, int>();

        _fruitImage = transform.GetChild(0).GetComponent<Image>();
        _animator = GetComponent<Animator>();
        _button = GetComponent<Button>();

        _gameController = FindObjectOfType<GameController>();
    }

    public void CellShowed()
    {
        _gameController.OpeningCellsIndexed.Remove(_buttonIndex);

        CellIndexedStateEvent.Invoke(_buttonIndex, _spriteId);
    }

    public void RandomCellShowed()
    {
        _gameController.OpeningCellsIndexed.Remove(_buttonIndex);
    }

    public void CellHided()
    {
        CellHideEvent.Invoke();
    }

    public void HideCell()
    {
        _button.interactable = true;
        _animator.SetBool("Hide", true);
        _animator.SetBool("Show", false);
        _cellState = CellState.Hide;
        //CellHideEvent.Invoke();
    }

    public void ShowCell()
    {
        _gameController.OpeningCellsIndexed.Add(_buttonIndex);
        _button.interactable = false;
        _animator.SetBool("Hide", false);
        _animator.SetBool("Show", true);
        _cellState = CellState.Show;
        //CellStateEvent.Invoke(_cellState);
    }

    public void ShowCellRandom(int buttonIndex)
    {
        _buttonIndex = buttonIndex;
        _gameController.OpeningCellsIndexed.Add(_buttonIndex);
        _button.interactable = false;
        _animator.SetBool("Hide", false);
        _animator.SetBool("RandomShow", true);
        _cellState = CellState.Show;
    }

    public void ShowCell(int buttonIndex)
    {
        _buttonIndex = buttonIndex;
        _gameController.OpeningCellsIndexed.Add(_buttonIndex);
        _button.interactable = false;
        _animator.SetBool("Hide", false);
        _animator.SetBool("Show", true);
        _cellState = CellState.Show;
        //CellIndexedStateEvent.Invoke(buttonIndex, _spriteId);
    }

    private void OnDestroy()
    {
        CellHideEvent.RemoveAllListeners();
        CellIndexedStateEvent.RemoveAllListeners();
    }
}
