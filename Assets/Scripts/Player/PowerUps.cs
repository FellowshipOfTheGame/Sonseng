using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour {
    public static PowerUps instance;
    public delegate void OnPSwtichActivatedHandler();
    public event OnPSwtichActivatedHandler OnPSwtichActivated;

    [Tooltip("Coin prefab in which obstacles will be transformed when p-switch is activated")]
    public GameObject coinPrefab;

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
    [Space(5)]
    [SerializeField] public bool shield;
    [SerializeField] float shieldTime;

    public bool Mirror => mirror;
    public bool Magnet => magnet;
    public bool Shield => shield;

    private void Start() {
        // Signleton
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(this.gameObject);
        }
    }

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
            case "Shield":
                StopCoroutine(ShieldActivate());
                StartCoroutine(ShieldActivate());
                break;
            case "P-Switch":
                ActivatePSwitch();
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

    /// <summary>
    /// Enables shield and disables it after shieldTime
    /// </summary>
    /// <returns>Power up duration</returns>
    private IEnumerator ShieldActivate() {
        shield = true;
        yield return new WaitForSeconds(shieldTime);
        shield = false;
    }

    public void ShieldDeactivate() {
        shield = false;
    }


    private void ActivatePSwitch() {
        OnPSwtichActivated?.Invoke();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        if (magnet) {
            Gizmos.DrawWireSphere(this.transform.position, magnetRadius);
        }
    }
}