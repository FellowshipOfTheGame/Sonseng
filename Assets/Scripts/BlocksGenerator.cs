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
    private float _timeToSpawn { get => 10 / Speed; }
    private const string _spawnFuncName = "SpawnObjects";

    private void Awake()
    {
        if (Instance)
            Destroy(gameObject);
        else Instance = this;

        _pooler = GetComponent<SimpleObjectPooler>();
        _pooler.Initialization(objects_prefabs);
        InvokeRepeating(_spawnFuncName, _timeToSpawn, _timeToSpawn);
    }

    /// <summary>
    /// Spawns all objects from matrix
    /// </summary>
    private void SpawnObjects()
    {
        SpawnableMatrix m = matrices[Random.Range(0, matrices.Length)];
        if (!m.CheckValid(spawn_points.Length))
        {
            return;
        }

        if (!m.CheckCompability(_lastMatrix))
        {
            return;
        }

        _lastMatrix = m;

        for(int i = 0; i < m.w; i++)
        {
            for(int j = 0; j < m.h; j++)
            {
                int index = i * m.w + j;
                if(m.matrix[index] == ObjectGroup.None)
                {
                    continue;
                }
                else
                {
                    GameObject prefab;
                    prefab = objects_prefabs[Random.Range((int)0, (int)objects_prefabs.Length)];
                    prefab = _pooler.GetObject(prefab);
                    if(prefab == null)
                    {
                        Debug.LogWarning("Prefab not found");
                        continue;
                    }
                    prefab.transform.position = spawn_points[index].position;
                    prefab.SetActive(true);
                }
            }
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
