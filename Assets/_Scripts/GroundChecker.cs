using KBCore.Refs;
using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace CharacterControllerFactory {

    public class GroundChecker : MonoBehaviour {
        [SerializeField] float groundDistance = 0.08f;
        [SerializeField] LayerMask groundLayers;

        public bool IsGrounded { get; private set; }

        private void Update() {
            IsGrounded = Physics.SphereCast(transform.position, groundDistance, Vector3.down, out _, groundDistance, groundLayers);
        }
    }
   
}