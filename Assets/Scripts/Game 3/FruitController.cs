using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FruitController : MonoBehaviour
{
    private Image _image;

    private Animator _animator;

    [SerializeField]
    private Image _fruitBackImg;

    [SerializeField]
    private List<Sprite> _backSprites = new();

    [HideInInspector]
    public UnityEvent FruitIsCatch;

    private bool _isCatch = false;

    private bool _isActive = false;

    private float _activeTime = 1.0f;

    public bool IsActive { get { return _isActive; } }

    private void Awake()
    {
        FruitIsCatch = new UnityEvent();
        _image = GetComponent<Image>();
        _animator = GetComponent<Animator>();
    }

    public void ActiveFruit(Sprite img)
    {
        if (!_isCatch)
        {
            _fruitBackImg.sprite = _backSprites[0];
            _animator.SetBool("Reduce", false);
            _animator.SetBool("Expand", true);
            _isCatch = true;
            _isActive = false;
            _image.sprite = img;
            StartCoroutine(FruitCoroutine());
        }
    }

    private IEnumerator FruitCoroutine()
    {
        while (_isCatch)
        {


            yield return new WaitForSeconds(_activeTime);

            _isCatch = false;
            _fruitBackImg.sprite = _backSprites[1];
            _animator.SetBool("Reduce", true);
            _animator.SetBool("Expand", false);
        }

    }

    public void FruitCatch()
    {
        Debug.Log("Press!");

        if (_isCatch)
        {
            _fruitBackImg.sprite = _backSprites[1];
            _animator.SetBool("Reduce", true);
            _animator.SetBool("Expand", false);
            _isCatch = false;
            FruitIsCatch.Invoke();
        }
    }

    public void FruitHide()
    {
        _isActive = false;
    }

    private void OnDestroy()
    {
        FruitIsCatch.RemoveAllListeners();
    }
}
