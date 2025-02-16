using UnityEngine;
using KBCore.Refs;
using Unity.Cinemachine;
using System;
using System.Collections;

namespace CharacterControllerFactory {
    public class CameraManager : MonoBehaviour {
        [SerializeField, Anywhere] InputReader input;
        [SerializeField, Anywhere] CinemachineCamera freeLookVCam;

        [Header("Settings")]
        [SerializeField, Range(0.5f, 3f)] float speedMultiplier;

        bool isRMBPressed;
        bool isDeviceMouse;
        bool cameraMovementLock;

        private void OnEnable() {
            input.Look += OnLook;
            input.EnableMouseControlCamera += OnEnableMouseControlCamera;
            input.DisableMouseControlCamera += OnDisableMouseControlCamera;
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
            freeLookVCam.transform.rotation = Quaternion.identity;
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

        private void OnLook(Vector2 arg0, bool arg1) {
            throw new NotImplementedException();
        }
    }

}