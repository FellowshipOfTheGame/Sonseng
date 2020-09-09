using UnityEngine;

public class Obstacle : DestructableObject {
    private void Start() {
        if(PowerUps.instance)
            PowerUps.instance.OnPSwtichActivated += TransformIntoCoin;
    }

    public override void Destroy() {
        // TODO add any effects here
        this.transform.parent.gameObject.SetActive(false);
    }

    public override void TransformIntoCoin() {
        // TODO add any effects here
        // TODO use object pooling
        Instantiate(PowerUps.instance.coinPrefab, this.transform.position, Quaternion.identity);
        this.transform.parent.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        PowerUps.instance.OnPSwtichActivated -= TransformIntoCoin;
    }
}