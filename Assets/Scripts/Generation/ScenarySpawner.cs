using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SimpleObjectPooler))]
public class ScenarySpawner : MonoBehaviour
{

    public enum SpawnConditionType
    {
        CharacterDistance,
        Time
    }

    [HideInInspector] public SpawnConditionType SpawnCondition = SpawnConditionType.CharacterDistance;

    [SerializeField] protected int InitialPrefabNumber = 1;
    [SerializeField] protected GameObject InitialPrefabs = null;
    [SerializeField] protected GameObject[] Prefabs = null;

    [SerializeField] protected float DistanceToPlayer = 20f;


    protected SimpleObjectPooler _pooler = null;
    protected Transform _lastTransform = null;
    protected Transform player = null;

    protected virtual void Awake()
    {
        // Prepare Spawn condition variables
        if (SpawnCondition.Equals(SpawnConditionType.CharacterDistance))
        {
            player = FindObjectOfType<PlayerMovement>().transform;
        }
        // Get pooler reference
        _pooler = GetComponent<SimpleObjectPooler>();
        _pooler.Initialization(Prefabs);

        // Get initial end position
        _lastTransform = InitialPrefabs.transform.Find("EndPosition");

        // Spawn Ahead Scenaries
        for(int i = 0; i < InitialPrefabNumber; i++)
        {
            InstantiateScenary();
        }
    }

    protected virtual void Update()
    {
        if (CheckSpawnCondition())
        {
            InstantiateScenary();
        }    
    }

    protected virtual bool CheckSpawnCondition()
    {
        // Don't spawn if game is paused
        if (GameManager.instance.IsGamePaused) return false;

        if (SpawnCondition.Equals(SpawnConditionType.CharacterDistance))
        {
            return (Vector3.Distance(player.position, _lastTransform.position) < DistanceToPlayer * InitialPrefabNumber);
        }

        return false;
    }

    protected virtual void InstantiateScenary()
    {
        // Get random scenary from pool
        GameObject scenary = null;
        int tries = 0;
        while (scenary == null)
        {
            if(tries >= Prefabs.Length)
            {
                Debug.Log($"No available object in pool of {gameObject.name}");
                return;
            }
            int random_index = UnityEngine.Random.Range((int)0, (int)Prefabs.Length);
            scenary = _pooler.GetObject(Prefabs[random_index]);
            if (scenary == null)
            {
                Debug.Log($"Couldnt find scenary {Prefabs[random_index].name} of index {random_index} from {gameObject.name}");
            }
            tries++;
        }

        // Update Last transform
        scenary.transform.position = _lastTransform.position;
        scenary.SetActive(true);

        // Update End Position
        _lastTransform = scenary.transform.Find("EndPosition");
    }
}
