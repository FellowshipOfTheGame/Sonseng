using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleObjectPooler : MonoBehaviour
{
    public List<GameObject> PoolableObjects = null;
    public bool isExpandable = true;
    //public Dictionary<string, bool> PossibleNames;

    private GameObject _pool;
    private HashSet<string> _possibleNames = new HashSet<string>();

    public void Initialization(GameObject[] poolables)
    {
        _pool = new GameObject("Object Pool");
        _pool.transform.SetParent(transform);
        Debug.Log("Initialized pooler");
        PoolableObjects = new List<GameObject>();

        foreach(var obj in poolables)
        {
            GameObject ins = Instantiate(obj, _pool.transform);
            ins.SetActive(false);
            PoolableObjects.Add(ins);
            _possibleNames.Add(ins.name);
        }
    }

    public GameObject GetObject(GameObject prefab)
    {
        if (!_possibleNames.Contains(prefab.name))
        {
            Debug.LogWarning($"Does not contain name {prefab.name}");
            return null;
        }

        foreach(var obj in PoolableObjects)
        {
            if(!obj.activeInHierarchy && obj.name.Equals(prefab.name))
            {
                return obj;
            }
        }

        if (isExpandable)
        {
            GameObject ins = Instantiate(prefab, _pool.transform);
            ins.SetActive(true);
            PoolableObjects.Add(ins);
            return ins;
        }

        return null;
    }
}
