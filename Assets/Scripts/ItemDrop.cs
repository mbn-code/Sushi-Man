using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    private Rigidbody2D RBody;
    public float DropForce = 5f;

    private void Start()
    {
        RBody = GetComponent<Rigidbody2D>();
        float randomX = Random.Range(-1f, 1f);
        Vector2 force = new Vector2(randomX, 1) * DropForce;
        RBody.AddForce(force, ForceMode2D.Impulse);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().GM.AddPoint();
            Destroy(gameObject);
        }
    }
}
