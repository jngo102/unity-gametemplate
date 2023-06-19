using UnityEngine;

public static class AnimatorExtensions {
    public static bool IsFinished(this Animator animator) => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1;
}
