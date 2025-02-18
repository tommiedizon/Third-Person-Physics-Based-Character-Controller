using Unity.VisualScripting;
using UnityEngine;

namespace CharacterControllerFactory {
    public class PlatformCollisionHandler : MonoBehaviour {
        // When we collide with a platform, we want that platform to become a parent of the player
        Transform platform;

        private void OnCollisionEnter(Collision other) {
            if(other.gameObject.CompareTag("MovingPlatform")) {
                // If the contact normal is pointing up, we've collided with the top of the platform
                ContactPoint contact = other.GetContact(0);
                if (contact.normal.y < 0.5f) return;

                platform = other.transform;
                transform.SetParent(platform);
            }
        }

        private void OnCollisionExit(Collision other) {
            if (other.gameObject.CompareTag("MovingPlatform")) {
                transform.SetParent(null);
                platform = null;
            }
        }



    }
}