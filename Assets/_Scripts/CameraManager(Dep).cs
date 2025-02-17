using UnityEngine;
using KBCore.Refs;
using Unity.Cinemachine;
using System;
using System.Collections;

namespace CharacterControllerFactory {
    public class CameraManager : MonoBehaviour {
        [SerializeField, Anywhere] InputReader input;
        [SerializeField, Anywhere] CinemachineCamera freeLookVCam;
        CinemachineOrbitalFollow orbitalFollow;

        [Header("Settings")]
        [SerializeField, Range(0.5f, 3f)] float speedMultiplier = 1f;

        bool isRMBPressed;
        bool cameraMovementLock;

        private void OnEnable() {
            input.Look += OnLook;
            input.EnableMouseControlCamera += OnEnableMouseControlCamera;
            input.DisableMouseControlCamera += OnDisableMouseControlCamera;
            orbitalFollow = freeLookVCam.GetComponent<CinemachineOrbitalFollow>();
        }

        private void OnDisable() {
            input.Look -= OnLook;
            input.EnableMouseControlCamera -= OnEnableMouseControlCamera;
            input.DisableMouseControlCamera -= OnDisableMouseControlCamera;
        }

        private void OnDisableMouseControlCamera() {
            isRMBPressed = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Reset the camera axis to prevent jumping when re-enabling mouse control
            orbitalFollow.HorizontalAxis.Value = 0f;
            orbitalFollow.VerticalAxis.Value = 0f;

        }

        private void OnEnableMouseControlCamera() {
            isRMBPressed = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            StartCoroutine(DisableMouseForFrame());
        }

        private IEnumerator DisableMouseForFrame() {
            // Used to prevent 'hiccups' when disabling the mouse
            cameraMovementLock = true;
            yield return new WaitForEndOfFrame();
            cameraMovementLock = false;
        }

        private void OnLook(Vector2 cameraMovement, bool isDeviceMouse) {
            if (cameraMovementLock) return;

            if (isDeviceMouse && !isRMBPressed) return;

            // If device is a mouse, use Time.fixedDeltaTime, otherwise use Time.deltaTime. Why?
            float deviceMultiplier = isDeviceMouse ? Time.fixedDeltaTime : Time.deltaTime;

            // Set the camera axis values
            orbitalFollow.HorizontalAxis.Value = cameraMovement.x * speedMultiplier * deviceMultiplier;
            orbitalFollow.VerticalAxis.Value = cameraMovement.y * speedMultiplier * deviceMultiplier;

        }
    }

}