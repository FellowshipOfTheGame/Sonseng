using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksGenerator : MonoBehaviour
{
    public static BlocksGenerator Instance = null;
    public float Speed { get => speed; set => SetSpeed(value); }

    [SerializeField] private SpawnableMatrix[] matrices;
    [SerializeField] private GameObject[] objects_prefabs;
    [SerializeField] private Transform[] spawn_points;
    [SerializeField] private float speed = 4f;

    private SpawnableMatrix _lastMatrix = null;
    private SimpleObjectPooler _pooler = null;
    private Dictionary<string, GameObject> _serializedObjects;
    private float _timeToSpawn { get => 10 / Speed; }
    private const string _spawnFuncName = "SpawnObjects";

    private void Awake()
    {
        if (Instance)
            Destroy(gameObject);
        else Instance = this;

        _pooler = GetComponent<SimpleObjectPooler>();
        _serializedObjects = new Dictionary<string, GameObject>();
    }

    private void Start()
    {
        _pooler.Initialization(objects_prefabs);
        foreach (var obj in objects_prefabs)
        {
            obj.name = obj.GetComponent<BlockDimensions>().GetSerialization();
            _serializedObjects[obj.name] = obj;
        }
        foreach(var sm in matrices)
        {
            sm.CalculateSubMatrices();
        }
        InvokeRepeating(_spawnFuncName, _timeToSpawn, _timeToSpawn);
    }

    /// <summary>
    /// Spawns all objects from matrix
    /// </summary>
    private void SpawnObjects()
    {
        SpawnableMatrix m;
        do
        {
            m = matrices[Random.Range(0, matrices.Length)];
            if (!m.CheckValid(spawn_points.Length))
            {
                continue;
            }

            if (!m.CheckCompability(_lastMatrix))
            {
                continue;
            }
            //Check if it has all possible patterns
            foreach (var sub in m.subMatrices)
            {
                if (!_serializedObjects.ContainsKey(sub.serial))
                {
                    Debug.LogWarning($"No {sub.serial}");
                    return;
                    //continue;
                }
            }
            break;
        } while (true);

        _lastMatrix = m;

        foreach(var subM in m.subMatrices)
        {
            GameObject prefab = _serializedObjects[subM.serial];
            prefab = _pooler.GetObject(prefab);
            if(prefab == null)
            {
                Debug.LogWarning("Prefab not found");
                continue;
            }
            prefab.transform.position = spawn_points[subM.index].position;
            Debug.Log($"spawning at position: {subM.index}");
            prefab.SetActive(true);
        }

    }

    public void SetSpeed(float value)
    {
        speed = value;
        if (IsInvoking())
            CancelInvoke();
        InvokeRepeating(_spawnFuncName, _timeToSpawn, _timeToSpawn);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
