using UnityEditor.Animations;
using UnityEngine;

public class PlayerVisual : MonoBehaviour {

    Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }
    public void ActivateIdleAnimation() {
        animator.Play("Idle");
    }

    public void TransitionToWalkAnimation(float fadeDuration) {
        animator.CrossFade("Walk", fadeDuration);
    }

    public void TransitionToIdleAnimation(float fadeDuration) {
        animator.CrossFade("Idle", fadeDuration);
    }

}
