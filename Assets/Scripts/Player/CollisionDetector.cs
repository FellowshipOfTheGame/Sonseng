using System;
using UnityEngine;

public class CollisionDetector : MonoBehaviour {
    public LayerMask CollisionMask;
    public Action OnDeath;
    private PowerUps powerUps;
    private PlayerMovement movement;

    private void Start() {
        powerUps = GetComponent<PowerUps>();
        movement = GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other) {
        // If collided with object of specified layer mask
        if ((CollisionMask.value & LayerMask.GetMask(LayerMask.LayerToName(other.gameObject.layer))) != 0) {
            if (powerUps.Shield) {
                // if collided with a destructable object, destroys it
                // if the object is not destructable, that means there was a collision with the wall, so moves the player away from wall
                if (other.TryGetComponent(out DestructableObject obj)) {
                    obj.Destroy();
                } else {
                    movement.MoveAwayFromWall();
                }

                powerUps.ShieldDeactivate();
            } else {
                OnDeath?.Invoke();
            }
        }
    }
}
