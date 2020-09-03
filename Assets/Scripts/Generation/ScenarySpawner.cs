using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarySpawner : MonoBehaviour
{
    public static ScenarySpawner Instance = null;
    public enum SpawnConditionType
    {
        CharacterDistance,
        Time
    }
    public SpawnConditionType ConditionType = SpawnConditionType.CharacterDistance;
    [SerializeField] private int InitialScenaryNumber = 1;
    [SerializeField] private GameObject InitialScenary = null;
    [SerializeField] private GameObject[] Scenaries = null;
    [SerializeField] private float DistanceToPlayer = 20f;
    public bool spawnActive = false;
    protected SimpleObjectPooler _pooler = null;
    protected Transform _lastTransform = null;
    protected Transform player = null;

    void Awake()
    {   
        if(Instance == null){
            Instance = this;
        }
        else{
            Destroy(this);
        }   

        // Prepare Spawn condition variables
        if (ConditionType.Equals(SpawnConditionType.CharacterDistance))
        {
            player = FindObjectOfType<PlayerMovement>().transform;
        }
        // Get pooler reference
        _pooler = GetComponent<SimpleObjectPooler>();
        _pooler.Initialization(Scenaries);

        // Get initial end position
        _lastTransform = InitialScenary.transform.Find("EndPosition");
    }

    private void Update()
    {
        if (CheckSpawnCondition() && spawnActive)
        {
            InstantiateScenary();
        }    
    }

    private bool CheckSpawnCondition()
    {
        if (ConditionType.Equals(SpawnConditionType.CharacterDistance))
        {
            return (Vector3.Distance(player.position, _lastTransform.position) < DistanceToPlayer * InitialScenaryNumber);
        }

        return false;
    }

    private void InstantiateScenary()
    {
        // Get random scenary from pool
        GameObject scenary = null;
        int tries = 0;
        while (scenary == null)
        {
            if(tries >= Scenaries.Length)
            {
                Debug.Log("No available object in pool");
                return;
            }
            int random_index = UnityEngine.Random.Range((int)0, (int)Scenaries.Length);
            scenary = _pooler.GetObject(Scenaries[random_index]);
            if (scenary == null)
            {
                Debug.Log($"Couldnt find scenary {Scenaries[random_index].name} of index {random_index}");
            }
            tries++;
        }

        // Update Last transform
        scenary.transform.position = _lastTransform.position;
        scenary.SetActive(true);

        // Update End Position
        _lastTransform = scenary.transform.Find("EndPosition");
    }

    public void StartSpawn()
    {
        // // Spawn Ahead Scenaries
        // for(int i = 0; i < InitialScenaryNumber; i++)
        // {
        //     InstantiateScenary();
        // }
        spawnActive = true;
    }
}
