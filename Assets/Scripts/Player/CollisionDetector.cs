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
            if (powerUps.Star || powerUps.Shield) {
                // if collided with a destructable object, destroys it (and count points, if with star power up)
                // if the object is not destructable, that means there was a collision with the wall, so moves the player away from wall
                if (other.TryGetComponent(out DestructableObject obj)) {
                    obj.Destroy();

                    // Add bonus points if destroyed using star power up
                    if (powerUps.Star) {
                        PowerUps.instance.AddStarBonus();
                    }
                } else {
                    movement.MoveAwayFromWall();
                }

                if (!powerUps.Star) {
                    powerUps.ShieldDeactivate(true);
                }
            } else {
                OnDeath?.Invoke();
            }
        }
    }
}
