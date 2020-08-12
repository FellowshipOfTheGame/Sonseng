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
    [SerializeField] float mirrorDuration;

    [Space(5)]
    [SerializeField] bool magnet;
    [SerializeField] float magnetDuration;
    [SerializeField] float magnetRadius;
    [SerializeField] float magnetForce;
    private Collider[] magnetAttractionResults = new Collider[25];
    private HashSet<Transform> coins = new HashSet<Transform>();

    [Space(5)]
    [SerializeField] bool star;
    [SerializeField] float starDuration;
    [SerializeField] float starBaseBonus;
    [SerializeField] float starBonusMultiplier;
    private float starCurrentBonus;

    [Space(5)]
    [SerializeField] bool shield;
    [SerializeField] float shieldDuration;

    public bool Mirror => mirror;
    public bool Magnet => magnet;
    public bool Star   => star;
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
                //TODO use object pooling
                Destroy(other.gameObject);
                break;

            // Power Ups
            case "Mirror":
                MirrorActivate();
                CancelInvoke(nameof(MirrorDeactivate));
                Invoke(nameof(MirrorDeactivate), mirrorDuration);
                break;

            case "Magnet":
                MagnetActivate();
                CancelInvoke(nameof(MagnetDeactivate));
                Invoke(nameof(MagnetDeactivate), magnetDuration);
                break;

            case "Star":
                StarActivate();
                CancelInvoke(nameof(StarDeactivate));
                Invoke(nameof(StarDeactivate), starDuration);
                break;

            case "Shield":
                ShieldActivate();
                CancelInvoke(nameof(ShieldDeactivate));
                Invoke(nameof(ShieldDeactivate), shieldDuration);
                break;

            case "P-Switch":
                PSwitchActivate();
                break;
        }
    }


    // Mirror
    private void MirrorActivate() {
        mirror = true;
    }

    private void MirrorDeactivate() {
        mirror = false;
    }

    // Magnet
    private void MagnetActivate() {
        magnet = true;
    }

    private void MagnetDeactivate() {
        magnet = false;
    }

    // Star
    private void StarActivate() {
        if (!star) {
            starCurrentBonus = starBaseBonus;
            star = true;
        }
    }

    private void StarDeactivate() {
        star = false;
    }

    /// <summary>
    /// Adds score bonus after destroying a block
    /// After each block, the bonus earned gets bigger
    /// </summary>
    public void AddStarBonus() {
        Scoreboard.instance.AddBonus(starCurrentBonus);
        Debug.Log("Object destroyed: " + starCurrentBonus);
        starCurrentBonus *= starBonusMultiplier;
    }

    // Shield
    private void ShieldActivate() {
        shield = true;
    }

    public void ShieldDeactivate() {
        shield = false;
    }

    // P-Switch
    private void PSwitchActivate() {
        OnPSwtichActivated?.Invoke();
    }


    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        if (magnet) {
            Gizmos.DrawWireSphere(this.transform.position, magnetRadius);
        }
    }
}