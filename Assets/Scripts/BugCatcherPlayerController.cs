using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class BugCatcherPlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float lookSensitivity = 0.1f;
    public Transform cameraTransform;

    private CharacterController _controller;
    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private float _cameraPitch = 0f;

    private float BugCatchingRaycastDistance = 3f;

    InputAction _catchBugAction;
    InputAction _moveAction;
    InputAction _lookAction;
    

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        // Bind input actions
        _catchBugAction = InputSystem.actions.FindAction("CatchBug");
        _moveAction = InputSystem.actions.FindAction("Move");
        _lookAction = InputSystem.actions.FindAction("Look");
    }

    public void OnMove(InputValue value) => _moveInput = value.Get<Vector2>();
    public void OnLook(InputValue value) => _lookInput = value.Get<Vector2>();

    public void CatchBug(GameObject bugObject)
    {
        Debug.Log("Raycast hit bug!");
        Destroy(bugObject);
    }

    void Update()
    {
        // Handle Rotation
        _lookInput = _lookAction.ReadValue<Vector2>();
        _cameraPitch -= _lookInput.y * lookSensitivity;
        _cameraPitch = Mathf.Clamp(_cameraPitch, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(_cameraPitch, 0, 0);
        transform.Rotate(Vector3.up * _lookInput.x * lookSensitivity);

        // Handle Movement
        _moveInput = _moveAction.ReadValue<Vector2>();
        Vector3 move = transform.right * _moveInput.x + transform.forward * _moveInput.y;
        _controller.Move(move * moveSpeed * Time.deltaTime);

        // Handle Catching Bugs
        float shouldTryCatchBug = _catchBugAction.ReadValue<float>();
        if (shouldTryCatchBug > 0.5) // Account for float uncertainty - should be 1 if pressed, 0 if no
        {
            LayerMask playerMask = LayerMask.GetMask("Player");
            LayerMask everythingButPlayerMask = ~playerMask;
            RaycastHit hitInfo;

            bool hitSomething = Physics.Raycast(cameraTransform.position, cameraTransform.TransformDirection(Vector3.forward), out hitInfo, BugCatchingRaycastDistance, everythingButPlayerMask);
            GameObject bugObject = hitSomething ? hitInfo.transform.gameObject : null;
            if (bugObject != null && bugObject.CompareTag("FreeBug")) 
            { 
                Debug.DrawRay(cameraTransform.position, cameraTransform.TransformDirection(Vector3.forward) * hitInfo.distance, Color.yellow, 2f);
                CatchBug(bugObject);
            }
        }
    }
}