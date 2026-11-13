using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    [SerializeField] private float smoothSpeed = 6f;
    [SerializeField] private Vector3 offset = new Vector3(0f, 1f, -10f);

    private void LateUpdate()
    {
        if (target == null) return;
        Vector3 desired = new Vector3(target.position.x, offset.y, offset.z);
        transform.position = Vector3.Lerp(transform.position, desired, smoothSpeed * Time.deltaTime);
    }
}
