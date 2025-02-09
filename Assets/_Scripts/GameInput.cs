using UnityEngine;
using System;

public class GameInput : MonoBehaviour {

    public static GameInput GameInputInstance {get; private set;}

    InputActionSystem inputActionSystem;

    private void Awake() {
        GameInputInstance = this;
    }
    private void Start() {
        inputActionSystem = new InputActionSystem();
        inputActionSystem.Player.Enable();
    }

    public Vector2 GetMovementVectorNormalized() {
        return inputActionSystem.Player.Move.ReadValue<Vector2>().normalized;
    }

}
