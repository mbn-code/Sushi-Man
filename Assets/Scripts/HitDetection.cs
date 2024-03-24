using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HitDetection : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit");
            if (SceneManager.GetActiveScene().name == "FightArea")
            {
                GetComponentInParent<PlayerController>().RemoveBallFromSpawner(collision.gameObject);
            } else
            {
                // Find DemonDog and apply damage
                Debug.Log(collision.gameObject.name);
                collision.gameObject.GetComponent<BossHandler>().ApplyDamage();
            }
        }
    }
}
