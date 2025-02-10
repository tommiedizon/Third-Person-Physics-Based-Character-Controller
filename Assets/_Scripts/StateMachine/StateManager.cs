using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
{   
    protected Dictionary<EState, BaseState<EState>> states = new Dictionary<EState, BaseState<EState>>();

    protected BaseState<EState> currentState;

    protected bool isTransitioningState = false;

    private void Start() {
        currentState.EnterState();
    }

    private void Update() {
        EState nextStateKey = currentState.GetNextState();

        if(!isTransitioningState && nextStateKey.Equals(currentState.StateKey)) {
            currentState.UpdateState();
        } else if (!isTransitioningState) {
            TransitionToState(nextStateKey);
        }
    }

    public void TransitionToState(EState stateKey) {
        isTransitioningState = true;
        currentState.ExitState();
        currentState = states[stateKey];
        currentState.EnterState();
        isTransitioningState = false;
    }

    private void OnTriggerEnter(Collider other) {
        currentState.OnTriggerEnter(other);
    }

    private void OnTriggerStay(Collider other) {
        currentState.OnTriggerStay(other);
    }

    private void OnTriggerExit(Collider other) {
        currentState.OnTriggerExit(other);
    }
}
