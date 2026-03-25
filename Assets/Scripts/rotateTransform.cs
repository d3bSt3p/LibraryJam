using UnityEngine;

public class rotateTransform : MonoBehaviour
{
    [SerializeField] Vector3 axis = Vector3.up;
    [SerializeField] float speed = 90f;
    [SerializeField] bool flipRotation = false;

    void Update()
    {
        float direction = flipRotation ? -1f : 1f;
        transform.Rotate(axis.normalized, speed * direction * Time.deltaTime);
    }
}
