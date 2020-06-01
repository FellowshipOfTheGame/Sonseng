using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private Rigidbody rBody;

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
    private int moveDirection = 0;
    private Vector3Int swipeDirection;

    private int NextLane {
        get { return nextLane; }
        set {
            // Keeps lane number between 0 and the number of lanes - 1 
            nextLane = Mathf.Clamp(value, 0, numberOfLanes - 1);
        }
    }

    private void Start() {
        rBody = GetComponent<Rigidbody>();
        rBody.useGravity = false;
        ResetCollider();
    }


    private void Update() {
        // If is falling, check if it is reaching ground
        if (!isGrounded && rBody.velocity.y < 0f) {
            isGrounded = CheckGround();
        }

#if !UNITY_ANDROID
        // In case of playing on a computer, gets keyboard input
        swipeDirection = GetKeyboardInput();
        if (swipeDirection != Vector3.zero) {
            OnSwipe(swipeDirection);
        }
#endif

        // Stops the movement when it arrives the destination lane
        if (ArrivedDestination()) {
            moveDirection = 0;
            rBody.velocity = new Vector3(0f, rBody.velocity.y, rBody.velocity.z);
            currentLane = NextLane;
        }
    }

    /// <summary>
    /// This method is called by Swipe Listener when it detects a swipe (but can be called manual as well).
    /// According to the swipe direction, move the player vertically or horizontally.
    /// </summary>
    /// <param name="swipeDirection"></param>
    public void OnSwipe(Vector3 swipeDirection) {
        // Horizontal movement
        if (swipeDirection.x != 0) {
            MoveSideways((int)swipeDirection.x);
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
    /// According to the direction passed as parameter, 
    /// </summary>
    /// <param name="direction"></param>
    private void MoveSideways(int direction) {
        /*  Resets movement and goes back to the current lane if input direction is the opposite of 
        current movement direction. Just increments NextLane otherwise. */
        if (moveDirection == -direction) {
            NextLane = currentLane;
        } else {
            NextLane += direction;
        }

        // Finds out the movement direction by checking the destination and the current x position
        if (lanes[NextLane].position.x > this.transform.position.x) {
            moveDirection = 1;
        } else {
            moveDirection = -1;
        }

        // Sets the velocity according to movement direction
        rBody.velocity = new Vector3(moveDirection * speed, rBody.velocity.y, rBody.velocity.z);
    }

    /// <summary>
    /// Add a force to make the player go up and resets the collider to normal size
    /// </summary>    
    private void Jump() {
        rBody.AddForce(Vector3.up * jumpForce);
        isGrounded = false;
        CancelInvoke(nameof(ResetCollider));
        ResetCollider();
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
        if (moveDirection == 1) {
            if (this.transform.position.x > lanes[NextLane].position.x)
                return true;
            else
                return false;
        } else if (moveDirection == -1) {
            if (this.transform.position.x < lanes[NextLane].position.x)
                return true;
            else
                return false;
        } else {
            return true;
        }
    }

    /// <summary>
    /// Gets keyboard arrows input and converts into a Vector3Int
    /// </summary>
    /// <returns>Pressed key direction</returns>
    private Vector3Int GetKeyboardInput() {
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