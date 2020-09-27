using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class RandomCollectableSystem : MonoBehaviour {
    public static RandomCollectableSystem Instance = null;

    [SerializeField] private List<GameObject> UnlockedCollectables = new List<GameObject>();
    [SerializeField] private GameObject CoinPrefab, magnetPrefab, shieldPrefab, starPrefab, coffeePrefab, mirrorPrefab, pbuttonPrefab, doublePrefab;

    private const int powerUpCount = 5;
    private const int debuffCount = 2;

    // Start is called before the first frame update
    private void Awake() {
#if UNITY_EDITOR
        Assert.IsNotNull(TimeToSpeedManager.instance, $"GameManager instance is null for {name}");
#endif
        // Singleton pattern
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Adds new collectable to the unlocked collectables list.
    /// </summary>
    /// <param name="prefab"></param>
    public void AddCollectable(string powerUp)
    {
        switch (powerUp)
        {
            case "double":
                if (!UnlockedCollectables.Contains(doublePrefab))
                    UnlockedCollectables.Add(doublePrefab);
                break;
            case "invincibility":
                if(!UnlockedCollectables.Contains(starPrefab))
                    UnlockedCollectables.Add(starPrefab);
                break;
            case "shield":
                if (!UnlockedCollectables.Contains(shieldPrefab))
                    UnlockedCollectables.Add(shieldPrefab);
                break;
            case "magnet":
                if (!UnlockedCollectables.Contains(magnetPrefab))
                    UnlockedCollectables.Add(magnetPrefab);
                break;
            case "p-button":
                if (!UnlockedCollectables.Contains(pbuttonPrefab))
                    UnlockedCollectables.Add(pbuttonPrefab);
                break;
            default:
                Debug.Log($"Trying to add {powerUp}, not available");
                break;
        }
    }

    /// <summary>
    /// Returns a random collectable prefab clone from the unlocked collectables.
    /// </summary>
    /// <returns></returns>
    public GameObject GetRandomCollectable()
    {
        // Get random index
        if (!PowerUps.instance.PowerUpActive)
        {
            int index = UnityEngine.Random.Range(0, powerUpCount + debuffCount + 1);
            if (index < UnlockedCollectables.Count)
            {
                return Instantiate(UnlockedCollectables[index]);
            }
            else if(index == 5 || index == 6)
            {
                return GetRandomDebuff();
            }
            else return GetCoin();
        }
        return GetCoin();
    }

    /// <summary>
    /// Returns a random powerUp prefab clone from the unlocked collectables.
    /// </summary>
    /// <returns></returns>
    public GameObject GetRandomPowerUp()
    {
        // Get random index
        if (!PowerUps.instance.PowerUpActive)
        {
            int index = UnityEngine.Random.Range(0, powerUpCount);
            if (index < UnlockedCollectables.Count)
                return Instantiate(UnlockedCollectables[index]);
            else return GetCoin();
        }
        return GetCoin();
    }

    /// <summary>
    /// Returns a random collectable prefab clone from the unlocked collectables.
    /// </summary>
    /// <returns></returns>
    public GameObject GetRandomDebuff()
    {
        // Get random index
        if (!PowerUps.instance.PowerUpActive)
        {
            // Choose random debuff. 50% each.
            return UnityEngine.Random.Range(0, debuffCount) == 0 ? Instantiate(coffeePrefab) : Instantiate(mirrorPrefab);
        }
        return GetCoin();
    }

    /// <summary>
    /// Returns a coin prefab clone.
    /// </summary>
    /// <returns></returns>
    public GameObject GetCoin()
    {
        return Instantiate(CoinPrefab, Vector3.up * 32, Quaternion.identity); // Spawn at high altitude to not get on Magnet Detection Area on Instantiation
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}