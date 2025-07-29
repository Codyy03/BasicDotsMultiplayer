using UnityEngine;
using UnityEngine.InputSystem.HID;

public class AnimationsController : MonoBehaviour
{
    [Header("Animation names")]
    public string idle;
    public string walk_forward;
    public string walk_backward;
    public string walk_left;
    public string walk_right;

    string currentAnimation;

    private Animator animator;

    [Tooltip("Transition time between animations (in seconds)")]
    public float transitionTime = 0.1f;

    [SerializeField] GameObject attackPoint;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Changes the animation if it's not already playing. Uses CrossFade for smooth transitions.
    public void ChangeAnimation(string animation)
    {
        if (string.IsNullOrEmpty(animation)) return;

        if (currentAnimation == animation) return;

        animator.CrossFade(animation, transitionTime);
        currentAnimation = animation;
    }
    public Animator GetAnimator()
    {
        return animator;
    }
}

