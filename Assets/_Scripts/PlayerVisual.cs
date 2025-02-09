using UnityEditor.Animations;
using UnityEngine;

public class PlayerVisual : MonoBehaviour {

    Animator animator;
    Player player;

    private void Start() {
        animator = GetComponent<Animator>();
        player = Player.PlayerInstance;
        animator.SetBool("isWalking", player.isWalking);
    }


    private void Update() {
        if (animator != null) {
            animator.SetBool("isWalking", player.isWalking);
        }
    }

}
