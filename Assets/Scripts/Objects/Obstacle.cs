using UnityEngine;

public class Obstacle : DestructableObject {

    [SerializeField] private ParticleSystem explosionContainer;
    private void Start() {
        if(PowerUps.instance)
            PowerUps.instance.OnPSwtichActivated += TransformIntoCoin;
    }

    public override void Destroy() {
        // TODO add any effects here
        explosionContainer.Play(true);
        this.transform.gameObject.SetActive(false);
    }

    public override void TransformIntoCoin() {
        // TODO add any effects here
        // TODO use object pooling
        Instantiate(PowerUps.instance.coinPrefab, this.transform.position, Quaternion.identity);
        Destroy();
    }

    private void OnDisable()
    {
        PowerUps.instance.OnPSwtichActivated -= TransformIntoCoin;
    }
}