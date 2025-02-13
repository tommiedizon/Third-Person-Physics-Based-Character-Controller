using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour {

    Rigidbody _rb;
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
        _rb = GetComponent<Rigidbody>();

        GameInput.GameInputInstance.OnSprintStarted += GameInputInstance_OnSprintStarted;
        GameInput.GameInputInstance.OnSprintCanceled += GameInputInstance_OnSprintCanceled;
    }

    private void FixedUpdate() {
        //HandleMovement();
        HandlePhysicsMovement();
    }

    // MAIN Helpers
    private void HandleMovement() {
        Vector2 inputVector = GameInput.GameInputInstance.GetMovementVectorNormalized();
        Vector3 forwardDir = ComputeForwardDirection();
        Vector3 perpDir = ComputePerpDirection(forwardDir);
        Vector3 moveDir = (inputVector.y * forwardDir - inputVector.x * perpDir).normalized;

        float playerSpeed = isSprinting ? sprintingSpeed : walkingSpeed;

        isWalking = (moveDir != Vector3.zero && !isSprinting);

        Vector3 movement = moveDir * playerSpeed;
        _rb.AddForce(movement, ForceMode.Acceleration);

        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }   

    private void HandlePhysicsMovement() {
        Vector2 inputVector = GameInput.GameInputInstance.GetMovementVectorNormalized();
        Vector3 forwardDir = ComputeForwardDirection();
        Vector3 perpDir = ComputePerpDirection(forwardDir);
        Vector3 moveDir = (inputVector.y * forwardDir - inputVector.x * perpDir).normalized;

        float playerSpeed = isSprinting ? sprintingSpeed : walkingSpeed;

        isWalking = (moveDir != Vector3.zero && !isSprinting);

        Vector3 targetVelocity = moveDir * playerSpeed;
        Vector3 deltaVelocity = targetVelocity - _rb.linearVelocity;
        float acceleration = 10f;
        deltaVelocity.y = 0; //ignore gravity for now



        _rb.AddForce(deltaVelocity * acceleration, ForceMode.Acceleration);

        if (moveDir != Vector3.zero) {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, targetRotation, Time.deltaTime * rotateSpeed));
        }

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
