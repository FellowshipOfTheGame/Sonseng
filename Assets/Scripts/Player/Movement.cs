using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    [SerializeField] Animator playerAnimator;
    [SerializeField] Animator horizontalAnimator;
    [SerializeField] Animator verticalAnimator;

    public AnimatorStateInfo verticalState;

    [SerializeField] Transform feet;
    [SerializeField] float groundCheckDistance;

    // private bool isFalling;
    private bool isGrounded;

    public bool IsFalling { get; set; }
    private bool IsGrounded {
        get { return isGrounded; }
        set {
            isGrounded = value;
            Debug.Log(isGrounded);

            if (isGrounded) {
                verticalAnimator.SetTrigger("ReachedGround");
            }
        }
    }

    
    private void Start() {
    }

    private void FixedUpdate() {
        if (IsFalling) {
            IsGrounded = Physics.Raycast(feet.position, Vector3.down, groundCheckDistance, LayerMask.GetMask("Ground"));
            Debug.DrawLine(feet.position, feet.position + Vector3.down * groundCheckDistance, Color.green, 0.1f);
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            horizontalAnimator.SetTrigger("MoveLeft");
        
        if (Input.GetKeyDown(KeyCode.RightArrow))
            horizontalAnimator.SetTrigger("MoveRight");
        
        if (Input.GetKeyDown(KeyCode.UpArrow))
            verticalAnimator.SetTrigger("Jump");

        if (Input.GetKeyDown(KeyCode.DownArrow))
            verticalAnimator.SetTrigger("Fall");

        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            if (isGrounded) {
                //TODO crouch
            } else {
                verticalAnimator.Play("Fall");
            }
        }
    }
}