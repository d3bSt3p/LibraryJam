using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float lookSensitivity = 0.1f;
    public Transform cameraTransform;

    private CharacterController _controller;
    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private float _cameraPitch = 0f;

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnMove(InputValue value) => _moveInput = value.Get<Vector2>();
    public void OnLook(InputValue value) => _lookInput = value.Get<Vector2>();

    void Update()
    {
        // Handle Rotation
        _cameraPitch -= _lookInput.y * lookSensitivity;
        _cameraPitch = Mathf.Clamp(_cameraPitch, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(_cameraPitch, 0, 0);
        transform.Rotate(Vector3.up * _lookInput.x * lookSensitivity);

        // Handle Movement
        Vector3 move = transform.right * _moveInput.x + transform.forward * _moveInput.y;
        _controller.Move(move * moveSpeed * Time.deltaTime);
    }
}