using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitBorder : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            Destroy(collision.gameObject);
        }
    }
}
