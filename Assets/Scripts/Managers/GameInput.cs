using System;
using CodeMonkey.Utils;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour {
    public static GameInput Instance { get; private set; }

    private CinemachineConfiner2D cinemachineConfiner2D;
    private CinemachineCamera cinemachineCamera;
    public PlayerInputActions playerInputActions;

    [SerializeField] private BoxCollider2D cameraBounds;
    [SerializeField] private float cameraMoveSpeed = 5f;
    [SerializeField] private float mouseDragMoveSpeed = 20f;
    
    private bool wasMousePressedOverUI = false;
    private Vector3 lastMousePosition;

    private void Awake() {
        Instance = this;
        cinemachineConfiner2D = GetComponent<CinemachineConfiner2D>();
        cinemachineCamera = GetComponent<CinemachineCamera>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Default.Enable();
    }

    public void Start() {
        // Calculate Camera Bounds
        Bounds gridBounds = GridManager.Instance.TryGetMainGrid().GetGridBounds();
        cameraBounds.size = new Vector2(gridBounds.size.x, gridBounds.size.y);
        cameraBounds.offset = gridBounds.center;
        cinemachineConfiner2D.InvalidateBoundingShapeCache();
        cinemachineConfiner2D.BoundingShape2D = cameraBounds;
        cinemachineCamera.transform.position = new Vector3(gridBounds.center.x, gridBounds.center.y, cinemachineCamera.transform.position.z);
    }

    private void Update() {
        // Handle keyboard input for camera movement
        HandleKeyboardCameraMovement();

        // Handle mouse drag input for camera movement
        HandleMouseDragCameraMovement();
    }

    private void HandleKeyboardCameraMovement() {
        Vector2 inputVector = GetKeyboardInputVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, inputVector.y, 0f);

        // Get the current confiner bounds
        Bounds confinerBounds = GetConfinerBounds();

        // Calculate the camera's current bounds based on its position
        Bounds cameraBounds = GetCameraBounds();

        // Check and adjust the movement based on confiner bounds
        if (cameraBounds.min.x < confinerBounds.min.x && moveDir.x < 0) {
            moveDir.x = 0; // Stop leftward movement
        }

        if (cameraBounds.max.x > confinerBounds.max.x && moveDir.x > 0) {
            moveDir.x = 0; // Stop rightward movement
        }

        if (cameraBounds.min.y < confinerBounds.min.y && moveDir.y < 0) {
            moveDir.y = 0; // Stop downward movement
        }

        if (cameraBounds.max.y > confinerBounds.max.y && moveDir.y > 0) {
            moveDir.y = 0; // Stop upward movement
        }

        // Apply the adjusted movement
        transform.position += moveDir * (cameraMoveSpeed * Time.deltaTime);
    }

    private Vector2 GetKeyboardInputVectorNormalized() {
        Vector2 inputVector = playerInputActions.Default.MoveCamera.ReadValue<Vector2>();
        inputVector = inputVector.normalized;

        return inputVector;
    }

    private void HandleMouseDragCameraMovement() {
        if (Mouse.current.leftButton.wasPressedThisFrame) {
            // Check if the pointer was over a UI element when the mouse button was pressed
            wasMousePressedOverUI = UtilsClass.IsPointerOverUI();
            // Store the last mouse position when the left mouse button is pressed
            lastMousePosition = Input.mousePosition;
        }

        if (Mouse.current.leftButton.isPressed && !wasMousePressedOverUI) {
            // Calculate the mouse movement delta
            Vector3 mouseDelta = lastMousePosition - Input.mousePosition;
            lastMousePosition = Input.mousePosition;

            // Move the camera based on the mouse delta
            float dragSensitivity = 10f;
            Vector3 moveDir = new Vector3(mouseDelta.x, mouseDelta.y, 0f) * (dragSensitivity * mouseDragMoveSpeed * Time.deltaTime);

            // Adjust the movement direction to be compatible with the world space
            moveDir = Camera.main.ScreenToWorldPoint(moveDir) - Camera.main.ScreenToWorldPoint(Vector3.zero);
            moveDir.z = 0f;

            // Get the current confiner bounds
            Bounds confinerBounds = GetConfinerBounds();

            // Calculate the camera's current bounds based on its position
            Bounds cameraBounds = GetCameraBounds();

            // Check and adjust the movement based on confiner bounds
            if (cameraBounds.min.x < confinerBounds.min.x && moveDir.x < 0) {
                moveDir.x = 0; // Stop leftward movement
            }
            if (cameraBounds.max.x > confinerBounds.max.x && moveDir.x > 0) {
                moveDir.x = 0; // Stop rightward movement
            }
            if (cameraBounds.min.y < confinerBounds.min.y && moveDir.y < 0) {
                moveDir.y = 0; // Stop downward movement
            }
            if (cameraBounds.max.y > confinerBounds.max.y && moveDir.y > 0) {
                moveDir.y = 0; // Stop upward movement
            }

            // Apply the adjusted movement
            transform.position += moveDir;
        }
    }

    private Bounds GetConfinerBounds() {
        if (cinemachineConfiner2D.BoundingShape2D) {
            return cinemachineConfiner2D.BoundingShape2D.bounds;
        }

        Debug.LogWarning("CinemachineConfiner2D has no bounding shape set!");
        return new Bounds(transform.position, Vector3.zero);
    }

    private Bounds GetCameraBounds() {
        // Assuming the camera is orthographic and 2D, calculate the bounds based on the camera size and position
        Camera mainCamera = Camera.main;
        float verticalSize = mainCamera.orthographicSize * 2f;
        float horizontalSize = verticalSize * mainCamera.aspect;

        Vector3 cameraSize = new Vector3(horizontalSize, verticalSize, 0f);
        return new Bounds(transform.position, cameraSize);
    }
}