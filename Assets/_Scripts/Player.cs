using System;
using Unity.Cinemachine;
using UnityEngine;

public class Player : MonoBehaviour {

    CharacterController controller;
    [SerializeField] CinemachineCamera cam;

    // Singleton
    public static Player PlayerInstance { get; private set; }

    public bool isWalking;

    // Player settings
    [SerializeField] float walkingSpeed = 6f;
    [SerializeField] float rotateSpeed = 6f;

    private void Awake() {
        PlayerInstance = this;
    }
    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
    }

    private void Update() {
        HandleMovement();
    }

    // Helper functions
    private void HandleMovement() {
        Vector2 inputVector = GameInput.GameInputInstance.GetMovementVectorNormalized();
        Vector3 forwardDir = ComputeForwardDirection();
        Vector3 perpDir = ComputePerpDirection(forwardDir);
        Vector3 moveDir = (inputVector.y * forwardDir - inputVector.x * perpDir).normalized;

        isWalking = (moveDir != Vector3.zero);
        Debug.Log(isWalking);

        controller.SimpleMove(moveDir * walkingSpeed);
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    private Vector3 ComputeForwardDirection() {
        Vector3 vect = transform.position - cam.transform.position;
        return new Vector3(vect.x, 0, vect.z).normalized;
    }

    private Vector3 ComputePerpDirection(Vector3 forwardDir) {
        return Vector3.Cross(forwardDir, Vector3.up).normalized;
    }


}
