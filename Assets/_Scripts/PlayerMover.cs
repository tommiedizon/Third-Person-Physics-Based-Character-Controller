using System;
using UnityEngine;

namespace CharacterControllerFactory
{

    [RequireComponent (typeof (Rigidbody), typeof(CapsuleCollider))]
    public class PlayerMover : MonoBehaviour {
        #region Fields
        [Header("Collider Settings")]
        [Range(0f, 1f)] [SerializeField] float stepHeightRatio = 0.1f;
        [SerializeField] float colliderHeight = 2f;
        [SerializeField] float colliderThickness = 1f;
        [SerializeField] Vector3 colliderOffset = Vector3.zero;

        // References
        Rigidbody rb;
        Transform tr;
        CapsuleCollider col;
        RaycastSensor sensor;

        bool isGrounded;
        float baseSensorRange;
        Vector3 currentGroundAdjustmentVelocity; // Velocity to adjust player position to maintain ground contact
        int currentLayer;

        [Header("Sensor Settings:")]
        [SerializeField] bool isInDebugMode;
        bool isUsingExtendedSensorRange = true; // Use extended range for smoother ground transitions
        #endregion

        private void Awake() {
            Setup();
            RecalculateColliderDimensions();
        }

        private void OnValidate() {
            // Called when script is loaded or value changes in the inspector
            if (gameObject.activeInHierarchy)
                RecalculateColliderDimensions();
        }


        private void RecalculateColliderDimensions() {
            if (col == null) {
                // we're in editor mode - Setup() hasn't been called yet
                Setup();
            }

            col.height = colliderHeight * (1f - stepHeightRatio);
            col.radius = colliderThickness / 2f;
            col.center = colliderOffset * colliderHeight + new Vector3(0f, stepHeightRatio * col.height / 2f, 0f);

            if (col.height / 2f < col.radius) { // to maintain a valid capsule shape
                col.radius = col.height / 2f;
            }

            RecalibrateSensor();
        }

        private void RecalibrateSensor() {
            sensor ??= new RaycastSensor(tr); // ??= is the 'null-coalescing assignment' operator. assigns a val to var iff var = null.
            sensor.SetCastOrigin(col.bounds.center); // centre of the world-space axis-aligned bounding box of the collider
            sensor.SetCastDirection(RaycastSensor.CastDirection.Down);
            RecalculateSensorLayerMask();

            const float safetyDistanceFactor = 0.001f; // Small factor added to prevent clipping issues when sensor range is calculated

            // vv don't know what this stuff does yet vv
            float length = colliderHeight * (1f - stepHeightRatio) * 0.5f + colliderHeight * stepHeightRatio;
            baseSensorRange = length * (1f + safetyDistanceFactor) * tr.localScale.x;
            sensor.castLength = length * tr.localScale.x;
        }

        private void RecalculateSensorLayerMask() {
            // TUFF FUNCTION (SIT DOWN AND UNDERSTAND)

            int objectLayer = gameObject.layer; // this gameObject's layer
            int layerMask = Physics.AllLayers; // constant = 1, all bits set to 1 in a 32 bit int

            for (int i = 0; i < 32; i++) { // iterate over all 32 layers
                // use Unity's collision matrix settings to determine whether this object layer is set to ignore layer
                if (Physics.GetIgnoreLayerCollision(objectLayer, i)) {
                    layerMask &= ~(1 << i); // wtf goin on!?!?
                }

            }

            int ignoreRaycastLayer = LayerMask.NameToLayer("Ignore Raycast");
            layerMask &= ~ignoreRaycastLayer; // Clear the associated bit in the layermask

            sensor.layerMask = layerMask;
            currentLayer = objectLayer;
        }
    
        private void Setup() {
            tr = transform; 
            rb = GetComponent<Rigidbody>();
            col = GetComponent<CapsuleCollider>();

            rb.freezeRotation = false;
            rb.useGravity = false;
        }

        public void CheckForGround() {
            if (currentLayer != gameObject.layer) {
                RecalculateSensorLayerMask();
            }

            currentGroundAdjustmentVelocity = Vector3.zero;
            sensor.castLength = isUsingExtendedSensorRange 
                ? baseSensorRange + colliderHeight * tr.localScale.x * stepHeightRatio 
                : baseSensorRange;
            sensor.Cast();

            isGrounded = sensor.HasDetectedHit();
            if (!isGrounded) return;

            float distance = sensor.GetDistance();
            float upperLimit = colliderHeight * tr.localScale.x * (1f - stepHeightRatio) * 0.5f;
            float middle = upperLimit + colliderHeight * tr.localScale.x * stepHeightRatio; // where we want the player's feet to be relative to the ground
            float distanceToGo = middle - distance;

            currentGroundAdjustmentVelocity = tr.up * (distanceToGo / Time.fixedDeltaTime);
        }

        public void SetVelocity(Vector3 velocity) => rb.linearVelocity = velocity + currentGroundAdjustmentVelocity;
        public void SetExtendedSensorRange(bool isExtended) => isUsingExtendedSensorRange = isExtended;

        public bool IsGrounded() => isGrounded;
        public Vector3 GetGroundNormal() => sensor.GetNormal();
    }
}
