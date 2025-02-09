using UnityEngine;

public class Player : MonoBehaviour {

    CharacterController characterController;
    [SerializeField] Transform playerCamera;
    Vector3 forwardDir;
    Vector3 perpDir;
    Vector3 moveDir;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();

        GameInput.GameInputInstance.OnMovePerformed += GameInputInstance_OnMovePerformed;
        GameInput.GameInputInstance.OnMoveCanceled += GameInputInstance_OnMoveCanceled;
    }

    private void Update() {
        characterController.SimpleMove(forwardDir);
    }

    // Event listeners
    private void GameInputInstance_OnMovePerformed(object sender, GameInput.OnMovePerformedEventArgs e) {
        HandleMovement(e.normalizedMoveDir);
    }

    private void GameInputInstance_OnMoveCanceled(object sender, System.EventArgs e) {
        HandleMovement(Vector2.zero);
    }

    // Helper functions
    private void HandleMovement(Vector2 rawPlayerInput) {
        forwardDir = ComputeForwardDirection(playerCamera);
        perpDir = ComputePerpDirection(forwardDir);

        moveDir = rawPlayerInput.x * forwardDir + rawPlayerInput.y * perpDir;
    }

    private Vector3 ComputeForwardDirection(Transform playerCamera) {
        Vector3 vect = transform.position - playerCamera.transform.position;
        return new Vector3(vect.x, 0, vect.z);
    }

    private Vector3 ComputePerpDirection(Vector3 forwardDir) {
        return Vector3.Cross(forwardDir, Vector3.up);
    }


}
