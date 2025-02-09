using UnityEngine;
using System;

public class GameInput : MonoBehaviour {

    public static GameInput GameInputInstance {get; private set;}

    InputActionSystem inputActionSystem;

    public class OnMovePerformedEventArgs : EventArgs {
        public Vector2 normalizedMoveDir;
    }

    public event EventHandler<OnMovePerformedEventArgs> OnMovePerformed;
    public event EventHandler OnMoveCanceled;

    private void Awake() {
        GameInputInstance = this;
    }
    private void Start() {
        inputActionSystem = new InputActionSystem();
        inputActionSystem.Player.Enable();
        inputActionSystem.Player.Move.performed += Move_performed;
        inputActionSystem.Player.Move.canceled += Move_canceled;
    }

    private void Move_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnMoveCanceled?.Invoke(this, EventArgs.Empty);
    }

    private void Move_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnMovePerformed?.Invoke(this, new OnMovePerformedEventArgs {
            normalizedMoveDir = inputActionSystem.Player.Move.ReadValue<Vector2>()
        });   
    }
}
