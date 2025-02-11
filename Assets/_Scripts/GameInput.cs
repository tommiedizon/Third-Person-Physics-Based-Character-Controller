using UnityEngine;
using System;

public class GameInput : MonoBehaviour {

    public static GameInput GameInputInstance {get; private set;}

    InputActionSystem inputActionSystem;

    public event EventHandler OnSprintStarted;
    public event EventHandler OnSprintCanceled;

    private void Awake() {
        GameInputInstance = this;
    }
    private void Start() {
        inputActionSystem = new InputActionSystem();
        inputActionSystem.Player.Enable();

        inputActionSystem.Player.Sprint.started += Sprint_started;
        inputActionSystem.Player.Sprint.canceled += Sprint_canceled;
    }

    private void Sprint_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnSprintCanceled?.Invoke(this, EventArgs.Empty);
    }

    private void Sprint_started(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnSprintStarted?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized() {
        return inputActionSystem.Player.Move.ReadValue<Vector2>().normalized;
    }

}
