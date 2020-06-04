using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour {
    [Space(5)]
    [SerializeField] bool mirror;
    [SerializeField] float mirrorTime;
    [Space(5)]
    [SerializeField] bool magnet;
    [SerializeField] float magnetTime;
    [SerializeField] float magnetRadius;
    [SerializeField] float magnetForce;
    private Collider[] magnetAttractionResults = new Collider[25];
    private HashSet<Transform> coins = new HashSet<Transform>();

    public bool Mirror => mirror;
    public bool Magnet => magnet;


    private void Update() {
        // Searches for new coins when the magnet is active
        if (magnet) {
            int size = Physics.OverlapSphereNonAlloc(this.transform.position, magnetRadius, magnetAttractionResults, LayerMask.GetMask("Coin"));
            foreach (var coin in magnetAttractionResults) {
                if (coin != null) {
                    coins.Add(coin.transform);
                }
            }
        }
    }

    private void FixedUpdate() {
        // Atract coins that are in the list
        foreach (var coin in coins) {
            coin.position = Vector3.MoveTowards(coin.position, this.transform.position, magnetForce * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter(Collider other) {
        switch (other.tag) {
            // Removes collected coins from coin list
            case "Coin":
                coins.Remove(other.transform);
                //TODO Let the coin destroy itself and count points
                Destroy(other.gameObject);
                break;

            // Power Ups
            case "Mirror":
                StopCoroutine(MirrorActivate());
                StartCoroutine(MirrorActivate());
                break;
            case "Magnet":
                StopCoroutine(MagnetActivate());
                StartCoroutine(MagnetActivate());
                break;
        }
    }

    /// <summary>
    /// Enables mirror power up and disables it after mirrorTime
    /// </summary>
    /// <returns>Power up duration</returns>
    private IEnumerator MirrorActivate() {
        mirror = true;
        yield return new WaitForSeconds(mirrorTime);
        mirror = false;
    }


    /// <summary>
    /// Enables magnet power up and disables it after magnetTime
    /// </summary>
    /// <returns>Power up duration</returns>
    private IEnumerator MagnetActivate() {
        magnet = true;
        yield return new WaitForSeconds(magnetTime);
        magnet = false;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        if (magnet) {
            Gizmos.DrawWireSphere(this.transform.position, magnetRadius);
        }
    }
}