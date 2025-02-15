using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour {

    [SerializeField] Transform cameraTransform;
    private MoveState moveState;
    GameInput gameInput;
    Rigidbody _rb;

    [SerializeField] Transform playerCentre;
    [SerializeField] float rayLength;
    [SerializeField] float buffer;
    [SerializeField] float springConstant;
    [SerializeField] float dampingConstant;
    [SerializeField] float torqueSpringConstant;
    [SerializeField] float torqueDampingConstant;


    private enum MoveState {
        Idle,
        Walk,
        Sprint,
        Jump
    }
    private void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start() {
        gameInput = GameInput.GameInputInstance;
        moveState = MoveState.Idle;
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        Vector2 playerInput = gameInput.GetMovementVectorNormalized();
        switch (moveState) {
            case MoveState.Idle:
                PlayerIdle(playerInput);
                break;
            case MoveState.Walk:
                PlayerWalk(playerInput);
                break;
            case MoveState.Sprint:
                PlayerSprint(playerInput);
                break;
        }
    }

    private void PlayerIdle(Vector2 playerInput) {
        if(playerInput != Vector2.zero) {
            moveState = MoveState.Walk;
        }
    }
    private float rotationSpeed = 10f;
    [SerializeField] private float walkingSpeed = 2f;
    private float acceleration = 10f;
    private void PlayerWalk(Vector2 playerInput) {

        // Spring code
        Ray ray = new(playerCentre.position, Vector3.down);
        Color rayColor = Color.red;

        if (Physics.Raycast(ray, out RaycastHit hit, rayLength)) {
            rayColor = Color.green;
            float surfaceHeight = hit.distance + buffer;
            Vector3 springForce = new Vector3(0f, rayLength - surfaceHeight, 0f);
            Vector3 yVel = new Vector3(0f, _rb.linearVelocity.y, 0f);

            _rb.AddForce(springConstant * springForce - dampingConstant * yVel);
        }

        Vector3 surfaceNormal = hit.normal;
        var springTorque = torqueSpringConstant * Vector3.Cross(_rb.transform.up, hit.normal);
        var dampTorque = torqueDampingConstant * -_rb.angularVelocity;
        _rb.AddTorque(springTorque + dampTorque, ForceMode.Acceleration);


        Debug.DrawRay(playerCentre.position, Vector3.down * rayLength, rayColor);

        // Translation code
        Vector3 forwardDir = transform.position - cameraTransform.transform.position;
        Vector3 perpDir = Vector3.Cross(forwardDir, Vector3.up);
        Vector3 moveDir = forwardDir * playerInput.y - perpDir * playerInput.x;

        // project onto surface
        moveDir = Vector3.ProjectOnPlane(moveDir, surfaceNormal);

        float speedDiff = walkingSpeed - _rb.linearVelocity.magnitude;
 
        _rb.AddForce(moveDir * speedDiff * acceleration, ForceMode.Acceleration);
        Debug.DrawRay(transform.position, moveDir * speedDiff, rayColor);


        if (moveDir != Vector3.zero) {
            Quaternion moveRotation = Quaternion.LookRotation(moveDir);
            _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, moveRotation, rotationSpeed * Time.fixedDeltaTime));
        }

    }
    private void PlayerSprint(Vector2 playerInput) {
        Debug.Log("Sprinting");
    }

}
