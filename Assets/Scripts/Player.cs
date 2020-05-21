using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private Rigidbody rBody;
    private Animator animator;

    // There are 5 lanes, but lanes 0 and 5 are not reachable.
    // Player starts on the middle lane
    private int numberOfLanes = 5;
    private int currentLane = 2;
    private int nextLane = 2;


    [Tooltip("Horizontal speed")]
    [SerializeField] float speed;
    [SerializeField] float rollTime;

    [Space(5f)]
    [SerializeField] float jumpForce;
    [Tooltip("Multiplies the gravity force")]
    [SerializeField] float gravityModifier;

    [Space(5)]
    [SerializeField] float fallFastSpeed;
    [SerializeField] float boxCastDistance;

    [Space(5f)]
    [SerializeField] Collider normalCollider;
    [SerializeField] Collider smallCollider;

    [SerializeField] Transform[] lanes;

    private bool isGrounded = true;
    private int moveDirection;
    private Vector3Int swipeDirection;

    private int NextLane {
        get { return nextLane; }
        set {
            // Keeps lane number between 0 and the number of lanes - 1 
            nextLane = Mathf.Clamp(value, 0, numberOfLanes - 1);
        }
    }

    private int MoveDirection {
        get { return moveDirection; }
        set {
            moveDirection = value;
            animator.SetInteger("MoveDirection", moveDirection);
        }
    }

    private void Start() {
        animator = GetComponentInChildren<Animator>();
        rBody = GetComponent<Rigidbody>();
        rBody.useGravity = false;
        ResetCollider();
    }


    private void Update() {
        // If is falling, check if it is reaching ground
        if (!isGrounded && rBody.velocity.y < 0f) {
            isGrounded = CheckGround();
        }

        swipeDirection = GetSwipe();

        // Horizontal movement
        if (swipeDirection.x != 0) {
            MoveSideways(swipeDirection.x);
        }

        // Vertical movement
        if (swipeDirection.y > 0) {
            if (isGrounded) {
                Jump();
            }
        } else if (swipeDirection.y < 0) {
            if (isGrounded) {
                Roll();
            } else {
                FallFast();
            }
        }

        // Stops the movement when it arrives the destination lane
        if (ArrivedDestination()) {
            MoveDirection = 0;
            rBody.velocity = new Vector3(0f, rBody.velocity.y, rBody.velocity.z);
            currentLane = NextLane;
        }
    }

    /// <summary>
    /// Applies customizable gravity
    /// </summary>
    private void FixedUpdate() {
        rBody.AddForce(Physics.gravity * gravityModifier);
    }

    /// <summary>
    /// Returns true or false depending on whether the player reached the ground or not
    /// </summary>
    /// <returns></returns>
    private bool CheckGround() {
        return Physics.BoxCast(this.transform.position, Vector3.one/2f, Vector3.down, Quaternion.identity, boxCastDistance);
    }


    /// <summary>
    /// Changes the active collider to the normal sized 
    /// </summary>
    private void ResetCollider() {
        normalCollider.enabled = true;
        smallCollider.enabled = false;
    }

    /// <summary>
    /// Changes the active collider to the small one
    /// </summary>
    private void ShrinkCollider() {
        normalCollider.enabled = false;
        smallCollider.enabled = true;
    }

    /// <summary>
    /// According to the direction passed as parameter, changes the next lane and moves towards it.
    /// </summary>
    /// <param name="direction"></param>
    private void MoveSideways(int direction) {
        /*  Resets movement and goes back to the current lane if input direction is the opposite of 
        current movement direction. Just increments NextLane otherwise. */
        if (MoveDirection == -direction) {
            NextLane = currentLane;
        } else {
            NextLane += direction;
        }

        // Finds out the movement direction by checking the destination and the current x position
        if (lanes[NextLane].position.x > this.transform.position.x) {
            MoveDirection = 1;
        } else {
            MoveDirection = -1;
        }

        // Sets the velocity according to movement direction
        rBody.velocity = new Vector3(MoveDirection * speed, rBody.velocity.y, rBody.velocity.z);
    }

    /// <summary>
    /// Add a force to make the player go up and resets the collider to normal size
    /// </summary>    
    private void Jump() {
        rBody.AddForce(Vector3.up * jumpForce);
        isGrounded = false;
        CancelInvoke(nameof(ResetCollider));
        ResetCollider();

        animator.SetTrigger("Jump");
    }

    /// <summary>
    /// Shrinks collider for some time 
    /// </summary>    
    private void Roll() {
        ShrinkCollider();
        Invoke(nameof(ResetCollider), rollTime);
        //TODO use animation event to reset collider
    }

    /// <summary>
    /// Makes the player start falling
    /// </summary>
    private void FallFast() {
        rBody.velocity = new Vector3(rBody.velocity.x, -fallFastSpeed, rBody.velocity.z);
    }

    /// <summary>
    /// Checks if player has already reached the destination lane comparing its x position with 
    /// the next lane x position
    /// </summary>
    /// <returns>Returns true if player has already reached the destination lane</returns>
    private bool ArrivedDestination() {
        if (MoveDirection == 1) {
            if (this.transform.position.x > lanes[NextLane].position.x)
                return true;
            else
                return false;
        } else if (MoveDirection == -1) {
            if (this.transform.position.x < lanes[NextLane].position.x)
                return true;
            else
                return false;
        } else {
            return true;
        }
    }

    /// <summary>
    /// Gets the swipe direction. Currently, only gets keyboard input.
    /// </summary>
    /// <returns>Returns the direction swipe</returns>
    private Vector3Int GetSwipe() {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) 
            return Vector3Int.left;
        if (Input.GetKeyDown(KeyCode.RightArrow)) 
            return Vector3Int.right;
        if (Input.GetKeyDown(KeyCode.UpArrow))
            return Vector3Int.up;
        if (Input.GetKeyDown(KeyCode.DownArrow))
            return Vector3Int.down;
        return Vector3Int.zero;
    }
}