using UnityEngine;

public class Player : MonoBehaviour {

    CharacterController characterController;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();

        GameInput.GameInputInstance.OnMovePerformed += GameInputInstance_OnMovePerformed;
    }

    private void GameInputInstance_OnMovePerformed(object sender, GameInput.OnMovePerformedEventArgs e) {
        HandleMovement(e.normalizedMoveDir);
    }

    private void HandleMovement(Vector2 rawPlayerInput) {
        Debug.Log(rawPlayerInput);
    }
}
