using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            GetComponentInParent<PlayerController>().RemoveBallFromSpawner(collision.gameObject);
        }
    }
}
