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
        _pool = Instantiate(new GameObject("Object Pool"), transform);
        PoolableObjects = new List<GameObject>();

        Debug.Log(poolables.Length);
        foreach(var obj in poolables)
        {
            GameObject ins = Instantiate(obj, _pool.transform);
            PoolableObjects.Add(ins);
            ins.SetActive(false);
            _possibleNames.Add(ins.name);
        }
    }

    public GameObject GetObject(GameObject prefab)
    {
        if (!_possibleNames.Contains(prefab.name) && !_possibleNames.Contains(prefab.name + "(Clone)"))
        {
            Debug.Log("Does not contain name");
            return null;
        }

        foreach(var obj in PoolableObjects)
        {
            if(!obj.activeInHierarchy && (obj.name.Equals(prefab.name) || obj.name.Equals(prefab.name + "(Clone)")))
            {
                obj.SetActive(true);
                return obj;
            }
        }

        if (isExpandable)
        {
            GameObject ins = Instantiate(prefab, _pool.transform);
            PoolableObjects.Add(ins);
            ins.SetActive(true);
            return ins;
        }

        return null;
    }
}
