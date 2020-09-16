﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class RandomCollectableSystem : MonoBehaviour
{
    public static RandomCollectableSystem Instance = null;

    [SerializeField] private List<GameObject> UnlockedCollectables = new List<GameObject>();
    [SerializeField] private GameObject CoinPrefab = null;
    
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
    }

    /// <summary>
    /// Adds new collectable to the unlocked collectables list.
    /// </summary>
    /// <param name="prefab"></param>
    public void AddCollectable(GameObject prefab)
    {
        UnlockedCollectables.Add(prefab);
    }

    /// <summary>
    /// Returns a random collectable prefab clone from the unlocked collectables.
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