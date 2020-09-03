using UnityEngine;

public class PlayerFollower : MonoBehaviour {
    [SerializeField] PlayerMovement player;

    private int currentPosition;
    private int nextPosition;
    private int moveDirection;
    
    [SerializeField] Transform[] positions;
    [SerializeField] float speed;

    private void OnEnable() {
        player.OnDestinationChange += UpdateDestination;
    }

    private void OnDisable() {
        player.OnDestinationChange -= UpdateDestination;
    }

    /// <summary>
    /// Moves the camera, if necessary
    /// </summary>
    private void FixedUpdate() {
        if (moveDirection != 0) {
            this.transform.position = Vector3.MoveTowards(this.transform.position, positions[nextPosition].position, speed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Constantly checks if the camera arrive its destination, stopping its movement if so
    /// </summary>
    private void Update() {
        if (ArrivedDestination()) {
            moveDirection = 0;
            currentPosition = nextPosition;
        }
    }

    /// <summary>
    /// Method called when player's destination changes
    /// Updates the move direction according to the changes
    /// </summary>
    /// <param name="nextLane">Player's destination lane</param>
    private void UpdateDestination(int nextLane) {
        // Camera has only 3 positions, so we clamp the lane number and converts to array index
        nextPosition = Mathf.Clamp(nextLane, 1, 3) - 1;

        if (this.transform.position.x > positions[nextPosition].position.x) {
            moveDirection = -1;
        } else {
            moveDirection = 1;
        }

        // rBody.velocity = new Vector3(MoveDirection * speed, rBody.velocity.y, rBody.velocity.z);
    }

    /// <summary>
    /// Checks if the camera has arrived its next position
    /// </summary>
    /// <returns>True if arrived, else, false</returns>
    private bool ArrivedDestination() {
        if (moveDirection == 1) {
            if (this.transform.position.x >= positions[nextPosition].position.x)
                return true;
            else
                return false;
        } else if (moveDirection == -1) {
            if (this.transform.position.x <= positions[nextPosition].position.x)
                return true;
            else
                return false;
        } else {
            return true;
        }
    }
}