using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUps : MonoBehaviour {
    public static PowerUps instance;
    public delegate void PSwitchEventHandler(float probability);
    public event PSwitchEventHandler OnPSwtichActivated;
    public event Action OnPowerPicked;

    [Tooltip("Coin prefab in which obstacles will be transformed when p-switch is activated")]
    public GameObject coinPrefab;

    [Tooltip("PlayerSoundEffects to play the pick up coin and powerup sounds")]
    [SerializeField] PlayerSoundEffects sfxPlayer;

    [Space(5)]
    [SerializeField] GameObject powerUpUI;
    [SerializeField] Image powerUpLogo;
    [SerializeField] Image powerUpBar;
    private bool powerUpActive;
    private float timeRemaining;
    private float maxTimeRemaining;

    [Space(5)]
    [SerializeField] bool mirror;
    [SerializeField] float mirrorDuration;
    [SerializeField] Sprite mirrorLogo;

    [Space(5)]
    [SerializeField] bool magnet;
    [SerializeField] float magnetDuration;
    [SerializeField] float magnetRadius;
    [SerializeField] float magnetForce;
    [SerializeField] Sprite magnetLogo;
    private Collider[] magnetAttractionResults = new Collider[25];
    private HashSet<Transform> coins = new HashSet<Transform>();

    [Space(5)]
    [SerializeField] bool star;
    [SerializeField] float starDuration;
    [SerializeField] float starBaseBonus;
    [SerializeField] float starBonusAddition;
    [SerializeField] float starBonusCooldown;
    [SerializeField] float starBonusCap;
    private bool canAddStarBonus = true;
    [SerializeField] Sprite starLogo;
    private float starCurrentBonus;

    [Space(5)]
    [SerializeField] bool shield;
    [SerializeField] float shieldDuration;
    [SerializeField] Sprite shieldLogo;

    [Space(5)]
    [SerializeField] bool doubleScore;
    [SerializeField] float doubleScoreDuration;
    [SerializeField] float doubleScoreMultiplier;
    [SerializeField] Sprite doubleScoreLogo;

    [Space(5)]
    [SerializeField] Animator ink;
    [SerializeField] float inkDuration;

    [Space(5)]
    [SerializeField] float probabilityOfTranformingIntoCoin;

    public bool PowerUpActive => powerUpActive;
    public bool Mirror => mirror;
    public bool Magnet => magnet;
    public bool Star   => star;
    public bool Shield => shield;

    private void Awake() {
        // Signleton
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(this.gameObject);
        }

        powerUpUI.SetActive(false);
        ink.gameObject.SetActive(false);
    }

    public void SetPowerUpValue(UserBackend.Upgrade upgrade) {
        switch (upgrade.upgradeName) {
            case "double":
                doubleScoreMultiplier = upgrade.baseValue * upgrade.multiplier;
                break;
            case "invincibility":
                starDuration = upgrade.baseValue * upgrade.multiplier;
                break;
            case "shield":
                shieldDuration = upgrade.baseValue * upgrade.multiplier;
                break;
            case "magnet":
                magnetDuration = upgrade.baseValue * upgrade.multiplier;
                break;
            case "coffee":
                inkDuration = upgrade.baseValue * upgrade.multiplier;
                break;
            case "mirror":
                mirrorDuration = upgrade.baseValue * upgrade.multiplier;
                break;
            case "p-button":
                probabilityOfTranformingIntoCoin = upgrade.baseValue * upgrade.multiplier;
                break;
            default:
                Debug.LogError("Unknown power up: " + upgrade.upgradeName);
                break;
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

        // Moves power up time bar if there is any power up active
        if (powerUpActive) {
            powerUpBar.fillAmount = timeRemaining / maxTimeRemaining;
            timeRemaining = Mathf.Clamp(timeRemaining - Time.deltaTime, 0f, maxTimeRemaining);

            if (timeRemaining <= 0f) {
                powerUpUI.SetActive(false);
                sfxPlayer.PowerUpEnd();
                powerUpActive = false;
            }
        }
    }

    private void FixedUpdate() {
        // Atract coins that are in the list
        foreach (var coin in coins) {
            if (coin.gameObject.activeSelf == true) // Don't recycled coins
                coin.position = Vector3.MoveTowards(coin.position, this.transform.position, (magnetForce + TimeToSpeedManager.instance.Speed) * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter(Collider other) {
        switch (other.tag) {
            // Removes collected coins from coin list
            case "Coin":
                coins.Remove(other.transform);
                Destroy(other.gameObject);
                Scoreboard.instance.AddCog();
                sfxPlayer.PickUpCoin();
                break;
            // Power Ups
            case "Mirror":
                MirrorActivate();
                CancelInvoke(nameof(MirrorDeactivate));
                Invoke(nameof(MirrorDeactivate), mirrorDuration);
                Destroy(other.gameObject);
                OnPowerPicked?.Invoke();
                sfxPlayer.PickUpPowerUp();
                break;

            case "Magnet":
                MagnetActivate();
                CancelInvoke(nameof(MagnetDeactivate));
                Invoke(nameof(MagnetDeactivate), magnetDuration);
                Destroy(other.gameObject);
                OnPowerPicked?.Invoke();
                sfxPlayer.PickUpPowerUp();
                break;

            case "Star":
                StarActivate();
                CancelInvoke(nameof(StarDeactivate));
                Invoke(nameof(StarDeactivate), starDuration);
                Destroy(other.gameObject);
                OnPowerPicked?.Invoke();
                sfxPlayer.PickUpPowerUp();
                break;

            case "Shield":
                ShieldActivate();
                CancelInvoke(nameof(ShieldDeactivate));
                Invoke(nameof(ShieldDeactivate), shieldDuration);
                Destroy(other.gameObject);
                OnPowerPicked?.Invoke();
                sfxPlayer.PickUpPowerUp();
                break;

            case "P-Switch":
                PSwitchActivate();
                Destroy(other.gameObject);
                OnPowerPicked?.Invoke();
                sfxPlayer.PickUpPowerUp();
                break;

            case "Ink":
                InkActivate();
                CancelInvoke(nameof(InkDeactivate));
                Invoke(nameof(InkDeactivate), inkDuration);
                Destroy(other.gameObject);
                OnPowerPicked?.Invoke();
                sfxPlayer.PickUpPowerUp();
                break;

            case "DoubleScore":
                DoubleScoreActivate();
                CancelInvoke(nameof(DoubleScoreDeactivate));
                Invoke(nameof(DoubleScoreDeactivate), doubleScoreDuration);
                Destroy(other.gameObject);
                OnPowerPicked?.Invoke();
                sfxPlayer.PickUpPowerUp();
                break;

        }
    }




    // Mirror
    private void MirrorActivate() {
        mirror = true;
        timeRemaining = mirrorDuration;
        maxTimeRemaining = timeRemaining;

        powerUpActive = true;
        powerUpLogo.sprite = mirrorLogo;
        powerUpUI.SetActive(true);
    }

    private void MirrorDeactivate() {
        mirror = false;
    }

    // Magnet
    private void MagnetActivate() {
        magnet = true;
        timeRemaining = magnetDuration;
        maxTimeRemaining = timeRemaining;

        powerUpActive = true;
        powerUpLogo.sprite = magnetLogo;
        powerUpUI.SetActive(true);
    }

    private void MagnetDeactivate() {
        magnet = false;
    }

    // Star
    private void StarActivate() {
        if (!star) {
            starCurrentBonus = starBaseBonus;
            star = true;

            timeRemaining = starDuration;
            maxTimeRemaining = timeRemaining;

            powerUpActive = true;
            powerUpLogo.sprite = starLogo;
            powerUpUI.SetActive(true);
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
        Debug.Log(canAddStarBonus);
        if (!canAddStarBonus)
            return;

        canAddStarBonus = false;
        Invoke(nameof(ResetStarBonus), starBonusCooldown);

        Scoreboard.instance.AddBonus(starCurrentBonus);
        Debug.Log("Object destroyed: " + starCurrentBonus);
        starCurrentBonus = Mathf.Clamp(starCurrentBonus + starBonusAddition, starBaseBonus, starBonusCap);
    }

    private void ResetStarBonus()
    {
        canAddStarBonus = true;
    }


    // Shield
    private void ShieldActivate() {
        shield = true;
        timeRemaining = shieldDuration;
        maxTimeRemaining = timeRemaining;

        powerUpActive = true;
        powerUpLogo.sprite = shieldLogo;
        powerUpUI.SetActive(true);
    }

    public void ShieldDeactivate() {
        shield = false;
        powerUpUI.SetActive(false);
    }

    // P-Switch
    private void PSwitchActivate() {
        OnPSwtichActivated?.Invoke(probabilityOfTranformingIntoCoin);
    }

    // DoubleScore
    private void DoubleScoreActivate() {
        doubleScore = true;
        timeRemaining = doubleScoreDuration;
        maxTimeRemaining = timeRemaining;

        powerUpActive = true;
        powerUpLogo.sprite = doubleScoreLogo;
        powerUpUI.SetActive(true);
        Scoreboard.instance.scoreMultiplier = doubleScoreMultiplier;
    }

    public void DoubleScoreDeactivate() {
        doubleScore = false;
        Scoreboard.instance.scoreMultiplier = 1f;
    }

    // Ink
    private void InkActivate() {
        ink.gameObject.SetActive(true);
        ink.Play("Start");
    }

    private void InkDeactivate() {
        ink.SetTrigger("Fade");
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        if (magnet) {
            Gizmos.DrawWireSphere(this.transform.position, magnetRadius);
        }
    }
}