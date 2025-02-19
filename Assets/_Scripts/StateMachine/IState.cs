using UnityEngine;

namespace CharacterControllerFactory {
    public interface IState {
        void OnEnter();
        void OnExit();
        void FixedUpdate();
        void Update();

    }
}

