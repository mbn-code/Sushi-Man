using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirHandler : MonoBehaviour
{
    [SerializeField] GameManager GM;
    [SerializeField] bool Boss;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Boss)
            {

            }
            else
            {
                PlayerPrefs.SetInt("Dif", PlayerPrefs.GetInt("Dif") + 1);
                PlayerPrefs.Save();
                GM.FadeToNext("FightArea");
            }
        }
    }
}
