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
    [SerializeField] float buffer = 1f;

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

    [SerializeField] float springConstant = 85f;
    [SerializeField] float dampingConstant = 25f;
    [SerializeField] float clearance = 1.1f;
    [SerializeField] Transform clearanceOriginPoint;
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

        // Spring action on Y MOVEMENT 
        CapsuleCollider collider = GetComponent<CapsuleCollider>();

        Vector3 origin = clearanceOriginPoint.position;
        Vector3 direction = Vector3.down;
        Ray ray = new Ray(origin, direction);
        
        Color rayColor = Color.red;

        // Perform the Raycast
        if (Physics.Raycast(origin, direction, out RaycastHit hit, clearance)) {
            Debug.Log("Hit: " + hit.collider.name);
            rayColor = Color.green; // Change color when hitting something

            float surfaceHeight = hit.distance + buffer;


            Vector3 springForce = new Vector3(0f, clearance - surfaceHeight, 0f);
            Vector3 yVel = new Vector3(0f, _rb.linearVelocity.y, 0f);

            _rb.AddForce(springConstant * springForce - dampingConstant * yVel);

        }

        Debug.DrawRay(origin, direction * clearance, rayColor);


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
