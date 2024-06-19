using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FruitFallController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rb;

    [SerializeField]
    private CircleCollider2D _collider;

    private float _duration = 10f;

    private float FruitFallDuration { get => _duration; set => _duration = value; }

    private void Start()
    {
        //_rb.AddForce(Vector2.down * _duration);
        _rb.velocity = Vector2.down * _duration;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            if (_collider != null)
                _collider.enabled = false;

            _rb.isKinematic = true;
        }
    }
}
