using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleObjectPooler : MonoBehaviour
{
    public List<GameObject> PoolableObjects = null;
    public bool isExpandable = true;

    private GameObject _pool;
    private HashSet<string> _possibleNames = new HashSet<string>();

    public void Initialization(GameObject[] poolables)
    {
        _pool = new GameObject($"[{name}] ObjectPool");
        //_pool.transform.SetParent(transform);
        _pool.transform.position = transform.position;
        Debug.Log($"Initialized pooler for  {name}");
        PoolableObjects = new List<GameObject>();

        foreach(var obj in poolables)
        {
            GameObject ins = Instantiate(obj, _pool.transform, false);
            ins.SetActive(false);
            ins.name = obj.name;
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
            GameObject ins = Instantiate(prefab, _pool.transform, false);
            ins.SetActive(false);
            ins.name = prefab.name;
            PoolableObjects.Add(ins);
            return ins;
        }

        return null;
    }

    public void AddObject(GameObject obj)
    {
        GameObject ins = Instantiate(obj, _pool.transform, false);
        ins.SetActive(false);
        ins.name = obj.name;
        PoolableObjects.Add(ins);
        _possibleNames.Add(ins.name);
    }
}
