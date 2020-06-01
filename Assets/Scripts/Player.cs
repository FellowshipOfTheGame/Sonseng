using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private Rigidbody rBody;

    // There are 5 lanes, but lanes 0 and 5 are not reachable.
    // Player starts on the middle lane
    private int numberOfLanes = 5;
    private int lane = 2;

    [Tooltip("Horizontal speed")]
    [SerializeField] float speed;
    [SerializeField] float toleranceDistance;
    [SerializeField] Transform[] lanes;

    private Vector3 newPosititon;

    private int Lane {
        get { return lane; }
        set {
            // Keeps lane number between 0 and the number of lanes - 1 
            lane = Mathf.Clamp(value, 0, numberOfLanes - 1);
        }
    }

    private void Start() {
        rBody = GetComponent<Rigidbody>(); 
        newPosititon = this.transform.position;
    }

    /// <summary>
    /// Handles input
    /// </summary>
    private void Update() {
        // Horizontal movement input
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            ChangeLane(Lane + 1);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            ChangeLane(Lane - 1);
        }

        // Vertical movement input
        // if (Input.GetKeyDown(KeyCode.UpArrow)) {
        //     Jump();
        // }
        // if (Input.GetKeyDown(KeyCode.DownArrow)) {
            // if (isGrounded) {
        //         Roll();
            // }
        // }        
    }

    /// <summary>
    /// Updates player position
    /// </summary>
    private void FixedUpdate() {
        rBody.MovePosition(newPosititon);
    }


    /// <summary>
    /// Starts the movement of the player, beggining the position interpolation.
    /// </summary>
    /// <param name="newLane">The numebr of the lane to where the player will move</param>
    private void ChangeLane(int nextLane) {
        Lane = nextLane;

        // Cancels current invoke and creates a new one
        CancelInvoke(nameof(InterpolatePositionX));
        InvokeRepeating(nameof(InterpolatePositionX), 0f, Time.fixedDeltaTime);
    }

    /// <summary>
    /// Updates the X axis of the next position.
    /// Automatically stops its repeating if it is close enough to destiny lane position.
    /// </summary>
    private void InterpolatePositionX() {
        float xPos = Mathf.Lerp(this.transform.position.x, lanes[Lane].position.x, speed * Time.fixedDeltaTime);
        newPosititon = new Vector3(xPos, newPosititon.y, newPosititon.z);

        if (Mathf.Abs(newPosititon.x - lanes[Lane].position.x) < toleranceDistance) {
            CancelInvoke(nameof(InterpolatePositionX));
        }
    }

    // private void Jump() {

    // }

    // private void Roll() {

    // }

    // private void FallFast() {

    // }
}