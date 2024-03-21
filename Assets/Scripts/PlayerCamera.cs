using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;

            // Debugging information
            Debug.Log("Target Position: " + target.position);
            Debug.Log("Desired Position: " + desiredPosition);
            Debug.Log("Smoothed Position: " + smoothedPosition);
            Debug.Log("Camera Position: " + transform.position);
        }
    }
}
