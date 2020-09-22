using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class RandomCollectableSystem : MonoBehaviour
{
    public static RandomCollectableSystem Instance = null;

    [SerializeField] private List<GameObject> UnlockedCollectables = new List<GameObject>();
    [SerializeField] private GameObject CoinPrefab, magnetPrefab, shieldPrefab, starPrefab, coffeePrefab, mirrorPrefab, pbuttonPrefab, doublePrefab;

    // Start is called before the first frame update
    private void Awake()
    {
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
            case "coffee":
                if (!UnlockedCollectables.Contains(coffeePrefab))
                    UnlockedCollectables.Add(coffeePrefab);
                break;
            case "mirror":
                if (!UnlockedCollectables.Contains(mirrorPrefab))
                    UnlockedCollectables.Add(mirrorPrefab);
                break;
            case "p-button":
                if (!UnlockedCollectables.Contains(pbuttonPrefab))
                    UnlockedCollectables.Add(pbuttonPrefab);
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
        if (UnlockedCollectables.Count > 0 && !PowerUps.instance.PowerUpActive)
        {

            int index = UnityEngine.Random.Range(0, UnlockedCollectables.Count);
            return Instantiate(UnlockedCollectables[index]);
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