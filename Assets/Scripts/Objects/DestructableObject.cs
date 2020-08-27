using UnityEngine;

public abstract class DestructableObject : MonoBehaviour {
    public abstract void Destroy();

    /// <summary>
    /// If there is no definition of this method in the child class, uses the Destroy method
    /// </summary>
    public virtual void TransformIntoCoin() {
        Destroy();
    }
}