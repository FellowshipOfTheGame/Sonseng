using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageOnContact : MonoBehaviour {
    public LayerMask CollisionMask;
    public Action OnDeath;
    private PowerUps powerUps;

    private void Start() {
        powerUps = GetComponent<PowerUps>();
    }

    private void OnTriggerEnter(Collider other) {
        // If collided with object of specified layer mask
        if ((CollisionMask.value & LayerMask.GetMask(LayerMask.LayerToName(other.gameObject.layer))) != 0) {
            if (powerUps.Shield) {
                powerUps.ShieldDeactivate();
                Destroy(other.gameObject);
            } else {
                OnDeath?.Invoke();
            }
        }
    }
}
