using UnityEngine;

public class Obstacle : DestructableObject {

    [SerializeField] private GameObject explosionContainer;
    private void Start() {
        if(PowerUps.instance)
            PowerUps.instance.OnPSwtichActivated += TransformIntoCoin;
    }

    public override void Destroy() {
        // TODO add any effects here
        explosionContainer.gameObject.SetActive(true);
        this.transform.parent.gameObject.SetActive(false);
    }

    public override void TransformIntoCoin() {
        // TODO add any effects here
        // TODO use object pooling
        explosionContainer.gameObject.SetActive(true);
        Instantiate(PowerUps.instance.coinPrefab, this.transform.position, Quaternion.identity);
        this.transform.parent.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        PowerUps.instance.OnPSwtichActivated -= TransformIntoCoin;
    }
}