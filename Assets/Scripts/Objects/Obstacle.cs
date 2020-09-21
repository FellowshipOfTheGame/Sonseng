using UnityEngine;

public class Obstacle : DestructableObject
{
    // TODO: Play Particles and remove loop
    [SerializeField] private GameObject explosionContainer;
    private void OnEnable()
    {    
        PowerUps.instance.OnPSwtichActivated += TransformIntoCoin;
    }

    public override void Destroy()
    {
        // TODO: Play Particles not in loop
        //explosionContainer.Play();
        gameObject.SetActive(false);
    }

    public override void TransformIntoCoin()
    {
        // TODO: Play Particles not in loop
        //explosionContainer.Play();
        Instantiate(PowerUps.instance.coinPrefab, this.transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        PowerUps.instance.OnPSwtichActivated -= TransformIntoCoin;
    }
}