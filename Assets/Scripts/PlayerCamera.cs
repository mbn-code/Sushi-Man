using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.825f;
    public Vector3 offset;
    [SerializeField] private Vector2 MaxBounds;
    [SerializeField] private Vector2 MinBounds;

    void LateUpdate()
    {

        if (target != null)
        {
            if(target.position.x > MaxBounds.x || target.position.x < MinBounds.x || target.position.y > MaxBounds.y || target.position.y < MinBounds.y)
            {
                return;
            }

            Vector3 desiredPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, transform.position.z);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
    }
}