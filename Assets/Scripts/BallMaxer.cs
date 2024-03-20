using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMaxer : MonoBehaviour
{
    [SerializeField]
    private float StopHeight = 0;

    private Rigidbody2D RBody;
    private Transform Trans;

    private void Start()
    {
        RBody = GetComponent<Rigidbody2D>();
        Trans = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        Vector2 vel = RBody.velocity;
        vel.x = 0;


        if (Trans.position.y >= StopHeight)
        {
            if(!(RBody.velocity.y < 0f))
                RBody.velocity = Vector2.zero;
        }
    }
}
