using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Transform transform;
    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.5f;
    private float dampingSpeed = 1.0f;
    Vector3 Initial;

    void Awake()
    {
        transform = GetComponent<Transform>();
    }

    void Update()
    {
        if (shakeDuration > 0f)
        {
            transform.localPosition = Initial + Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
        }
    }

    internal void Shake(float Time)
    {
        Initial = transform.localPosition;
        shakeDuration = Time;
    }
}
