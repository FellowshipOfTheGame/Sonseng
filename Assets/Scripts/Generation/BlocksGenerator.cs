using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksGenerator : MonoBehaviour
{
    public static BlocksGenerator Instance = null;
    public float Speed { get => speed; set => SetSpeed(value); }

    [Header("Speed Settings")]
    public AnimationCurve SpawnSpeedCurve = null;
    [SerializeField] private float speed = 4f;
    [SerializeField] private float spawnTimeConstant = 10f;

    [Header("Spawn Prefabs/Assets")]
    [SerializeField] private SpawnableMatrix[] matrices;
    [SerializeField] private GameObject[] objects_prefabs;
    [SerializeField] private Transform[] spawn_points;
    private SpawnableMatrix _lastMatrix = null;
    private SimpleObjectPooler _pooler = null;
    private Dictionary<string, GameObject> _serializedObjects;
    private float _timeToSpawn { get => spawnTimeConstant * SpawnSpeedCurve.Evaluate(Speed); }
    private const string _spawnFuncName = "SpawnObjects";
    [SerializeField] private CollisionDetector collisionDetector;

    private void Awake()
    {
        if (Instance)
            Destroy(gameObject);
        else Instance = this;

        _pooler = GetComponent<SimpleObjectPooler>();
        _serializedObjects = new Dictionary<string, GameObject>();

        collisionDetector.OnDeath += Stop;
    }

    private void Start()
    {
        //Populate object pool and serialize objects name's
        foreach (var obj in objects_prefabs)
        {
            obj.name = obj.GetComponent<BlockDimensions>().GetSerialization();
            _serializedObjects[obj.name] = obj;
        }
        _pooler.Initialization(objects_prefabs);

        // Calculate sub matrices for every matrix
        foreach(var sm in matrices)
        {
            sm.CalculateSubMatrices();
        }
        
    }

    public void StartSpawn() {
        // Invoke spawn function
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

    private void Stop() {
        speed = 0;
        CancelInvoke();
    }
}
