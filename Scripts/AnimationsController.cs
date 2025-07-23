using UnityEngine;
using UnityEngine.InputSystem.HID;

public class AnimationsController : MonoBehaviour
{
    [Header("Animation names")]
    public string idle;
    public string walk_forward;
    public string walk_backward;
  //  public string rotation_right;
   // public string rotation_left;
    public string run;
    public string jump;
    public string dead;
    public string attack;

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

    public void EnableAttack()
    {
        attackPoint.SetActive(true);

    }
    public void DisableAttack()
    {
        attackPoint.SetActive(false);

    }
}

