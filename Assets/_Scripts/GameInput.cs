using UnityEngine;
using System;

public class GameInput : MonoBehaviour {

    public static GameInput GameInputInstance {get; private set;}

    InputActionSystem inputActionSystem;

    public class OnMovePerformedEventArgs : EventArgs {
        public Vector2 normalizedMoveDir;
    }

    public event EventHandler<OnMovePerformedEventArgs> OnMovePerformed;

    private void Awake() {
        GameInputInstance = this;
    }
    private void Start() {
        inputActionSystem = new InputActionSystem();
        inputActionSystem.Player.Enable();
        inputActionSystem.Player.Move.performed += Move_performed;
    }

    private void Move_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnMovePerformed?.Invoke(this, new OnMovePerformedEventArgs {
            normalizedMoveDir = inputActionSystem.Player.Move.ReadValue<Vector2>()
        });   
    }
}
