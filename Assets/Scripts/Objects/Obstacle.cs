using UnityEngine;

public class Obstacle : DestructableObject {

    [SerializeField] private ParticleSystem explosionContainer;
    private void OnEnable() {
        if(PowerUps.instance)
            PowerUps.instance.OnPSwtichActivated += TransformIntoCoin;
    }

    private void OnDisable() {
        PowerUps.instance.OnPSwtichActivated -= TransformIntoCoin;
    }
    
    public override void Destroy() {
        explosionContainer.Play(true);
        this.transform.gameObject.SetActive(false);
    }

    public override void TransformIntoCoin(float probability) {
        if (Random.Range(0f, 1f) <= probability) 
            Instantiate(PowerUps.instance.coinPrefab, this.transform.position, Quaternion.identity);
        Destroy();
    }

}