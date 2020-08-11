using UnityEngine;

public class Obstacle : DestructableObject {
    //TODO
    public override void Destroy() {
        Debug.Log("Destroyed")   ;
    }

    //TODO
    public override void TransformIntoCoin() {
        Debug.Log("Transformed into a coin");
    }
}