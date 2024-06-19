using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class BasketController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _basketPositionInfo;

    [SerializeField]
    private ScoreCounter _scoreCounter;

    private AudioController _audioController;

    [SerializeField]
    private InputController _inputController;

    private Vector3 _basketPosition;

    public Vector3 BasketPosition { get => _basketPosition; set => _basketPosition = value; }

    private float previousSign;

    private void Start()
    {
        _audioController = FindObjectOfType<AudioController>();
        _basketPosition = transform.position;
        previousSign = Mathf.Sign(_basketPosition.x);
    }

    private void Update()
    {
        transform.position = _basketPosition;

        if (Mathf.Sign(_basketPosition.x) != Mathf.Sign(previousSign))
        {
            transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, -transform.rotation.z, transform.rotation.w);
        }

        previousSign = Mathf.Sign(_basketPosition.x);

        if (_basketPositionInfo != null)
            _basketPositionInfo.text = transform.position.ToString();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.CompareTag("Fruit"))
            {
                if (_audioController != null)
                    _audioController.PlayGoodSound();
                _scoreCounter.IncreaseScore();
                Destroy(collision.gameObject);
            }
        }
    }
}
