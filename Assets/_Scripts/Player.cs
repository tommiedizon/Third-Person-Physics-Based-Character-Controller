using UnityEngine;

public class Player : MonoBehaviour {

    CharacterController characterController;
    [SerializeField] Transform playerCamera;
    Vector3 forwardDir;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();

        GameInput.GameInputInstance.OnMovePerformed += GameInputInstance_OnMovePerformed;
    }

    private void GameInputInstance_OnMovePerformed(object sender, GameInput.OnMovePerformedEventArgs e) {
        HandleMovement(e.normalizedMoveDir);
    }

    private void HandleMovement(Vector2 rawPlayerInput) {
        //characterController.SimpleMove(forwardDir);
    }

    private Vector3 ComputeForwardDirection(Transform playerCamera) {
        Vector3 vect = transform.position - playerCamera.transform.position;
        return new Vector3(vect.x, 0, vect.z);
    }

    private void Update() {
        forwardDir = ComputeForwardDirection(playerCamera);
    }
}
