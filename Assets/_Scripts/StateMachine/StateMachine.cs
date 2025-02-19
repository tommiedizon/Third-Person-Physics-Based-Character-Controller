using System;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterControllerFactory {
    public class StateMachine {
        StateNode current; // Represents state and all of its transitions
        Dictionary<Type, StateNode> nodes;
        HashSet<ITransition> anyTransitions = new();

        public StateMachine() {
            this.nodes = new Dictionary<Type, StateNode>();
        }

        public void Update() {
            ITransition transition = GetTransition();

            if (transition != null) {
                ChangeState(transition.To);
            }

            current.State?.Update();
        }

        public void FixedUpdate() {
            current.State?.FixedUpdate();
        }

        public void SetState(IState state) {
            state.OnEnter();
            current = nodes[state.GetType()];
        }

        private void ChangeState(IState state) {
            if (state == current.State) return;

            var previousState = current.State;
            var nextState = nodes[state.GetType()].State;

            previousState?.OnExit();
            nextState?.OnEnter();
        }

        ITransition GetTransition() {
            foreach (var transition in anyTransitions) 
                if (transition.Condition.Evaluate()) { return transition; }

            foreach (var transition in current.Transitions) 
                if (transition.Condition.Evaluate()) { return transition; }

            return null;
        }

        public void AddTransition(IState from, IState to, IPredicate condition) {
            GetOrAddNode(from).AddTransition(GetOrAddNode(to).State  , condition);
        }

        public void AddAnyTransition(IState to, IPredicate condition) {
            anyTransitions.Add(new Transition(to, condition));
        }

        StateNode GetOrAddNode(IState state) {
           
            var node = nodes.GetValueOrDefault(state.GetType());

            if(node == null) {
                node= new StateNode(state);
                nodes.Add(state.GetType(), node);
            }

            return node;
        }

        private class StateNode {
            public IState State { get; }
            public HashSet<ITransition> Transitions { get; }

            public StateNode(IState state) {
                State = state;
                Transitions = new HashSet<ITransition>();
            }

            public void AddTransition(IState to, IPredicate condition) {
                Transitions.Add(new Transition(to, condition));
            }
        }
    }
}
