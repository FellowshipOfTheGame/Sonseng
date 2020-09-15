using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(SimpleObjectPooler))]
public class RandomCollectableSystem : MonoBehaviour
{
    public static RandomCollectableSystem Instance = null;

    [SerializeField] private List<GameObject> UnlockedCollectables = new List<GameObject>();
    [SerializeField] private GameObject CoinPrefab = null;
    [SerializeField] private SimpleObjectPooler Pool = null;
    
    // Start is called before the first frame update
    private void Awake()
    {
#if UNITY_EDITOR
        Assert.IsNotNull(GameManager.instance, $"GameManager instance is null for {name}");
#endif
        // Singleton pattern
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Initiate Object Pool
        Pool = GetComponent<SimpleObjectPooler>();
    }

    internal void Initialize()
    {
        Pool.Initialization(UnlockedCollectables.ToArray());
    }

    /// <summary>
    /// Adds new collectable to the unlocked collectables list and pool.
    /// </summary>
    /// <param name="prefab"></param>
    public void AddCollectable(GameObject prefab)
    {
        UnlockedCollectables.Add(prefab);
        Pool.AddObject(prefab);
    }

    /// <summary>
    /// Returns a random collectable prefab from the unlocked collectables pool.
    /// </summary>
    /// <returns></returns>
    public GameObject GetRandomCollectable()
    {
        // Get random index
        int index = UnityEngine.Random.Range(0, UnlockedCollectables.Count);
        //return Pool.GetObject(UnlockedCollectables[index]);
        return Instantiate(UnlockedCollectables[index]);
    }

    /// <summary>
    /// Returns a coin prefab clone.
    /// </summary>
    /// <returns></returns>
    public GameObject GetCoin()
    {
        return Instantiate(CoinPrefab);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
