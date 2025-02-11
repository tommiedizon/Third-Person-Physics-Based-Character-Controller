using System;
using Unity.Cinemachine;
using UnityEngine;

public class Player : MonoBehaviour {

    CharacterController controller;
    [SerializeField] CinemachineCamera cam;

    // Singleton
    public static Player PlayerInstance { get; private set; }

    public bool isWalking = false;
    public bool isSprinting = false;

    // Player settings
    [SerializeField] float walkingSpeed = 6f;
    [SerializeField] float sprintingSpeed = 20f;
    [SerializeField] float rotateSpeed = 6f; 

    private void Awake() {
        PlayerInstance = this;
    }
    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();

        GameInput.GameInputInstance.OnSprintStarted += GameInputInstance_OnSprintStarted;
        GameInput.GameInputInstance.OnSprintCanceled += GameInputInstance_OnSprintCanceled;
    }

    private void Update() {
        HandleMovement();
    }

    // MAIN Helpers
    private void HandleMovement() {
        Vector2 inputVector = GameInput.GameInputInstance.GetMovementVectorNormalized();
        Vector3 forwardDir = ComputeForwardDirection();
        Vector3 perpDir = ComputePerpDirection(forwardDir);
        Vector3 moveDir = (inputVector.y * forwardDir - inputVector.x * perpDir).normalized;

        float playerSpeed = isSprinting ? sprintingSpeed : walkingSpeed;

        isWalking = (moveDir != Vector3.zero && !isSprinting);

        controller.SimpleMove(moveDir * playerSpeed);
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    // Helpers
    private Vector3 ComputeForwardDirection() {
        Vector3 vect = transform.position - cam.transform.position;
        return new Vector3(vect.x, 0, vect.z).normalized;
    }

    private Vector3 ComputePerpDirection(Vector3 forwardDir) {
        return Vector3.Cross(forwardDir, Vector3.up).normalized;
    }


    // Event Listeners
    private void GameInputInstance_OnSprintCanceled(object sender, EventArgs e) {
        isSprinting = false;
    }

    private void GameInputInstance_OnSprintStarted(object sender, EventArgs e) {
        isSprinting = true;
    }
}
