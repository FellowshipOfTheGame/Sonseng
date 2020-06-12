using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjectGenerator : MonoBehaviour
{
    public static RandomObjectGenerator Instance = null;
    public float Speed { get => speed; set => SetSpeed(value); }

    [SerializeField] private GameObject[] objects_prefabs;
    [SerializeField] private Transform[] spawn_points;
    [SerializeField] private int maximum_spawn_per_tick;
    //[SerializeField] private float[] x_offsets = { -1f, 0f, 1f };
    //[SerializeField] private float z_offset = 10f;

    private float Time_to_spawn { get => 10 / Speed;}
    private float speed = 4f;

    protected Vector3 _lastSpawnPoint = Vector3.zero;
    protected Vector3 _newSpawnPoint = Vector3.zero;
    protected SimpleObjectPooler _pooler = null;

    private void Awake()
    {
        if (Instance)
            Destroy(gameObject);
        else Instance = this;

        _pooler = GetComponent<SimpleObjectPooler>();
        _pooler.Initialization(objects_prefabs);
        InvokeRepeating("SpawnRandomObjects", Time_to_spawn, Time_to_spawn);
    }
    
    /// <summary>
    /// Spawns from 1 to maximum_spawn_per_tick random objects on random locations
    /// </summary>
    private void SpawnRandomObjects()
    {
        _lastSpawnPoint = Vector3.zero;
        for(int i = Random.Range(0, maximum_spawn_per_tick); i < maximum_spawn_per_tick; i++)
        {
            do
            {
                _newSpawnPoint = spawn_points[Random.Range((int)0, (int)spawn_points.Length)].position;
            } while (_lastSpawnPoint == _newSpawnPoint);

            GameObject prefab = objects_prefabs[Random.Range((int)0, (int)objects_prefabs.Length)];

            prefab = _pooler.GetObject(prefab);
            if (prefab == null) continue;
            prefab.transform.position = _newSpawnPoint;
            //Instantiate(prefab, _newSpawnPoint, transform.rotation, transform);
            _lastSpawnPoint = _newSpawnPoint;
        }
    }

    public void SetSpeed(float value)
    {
        speed = value;
        if (IsInvoking())
            CancelInvoke();
        InvokeRepeating("SpawnRandomObjects", Time_to_spawn, Time_to_spawn);
    }

    private void OnDestroy()
    {
        if(Instance == this)
            Instance = null;
    }
}
