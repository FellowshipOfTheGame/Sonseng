using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LGO
{
    public GameObject[] Array = new GameObject[0];
    public int Length => Array.Length;

    public GameObject this[int i]
    {
        get { return Array[i]; }
        set { Array[i] = value; }
    }

    public LGO()
    {
        Array = new GameObject[0];
    }

    public LGO(int size)
    {
        Array = new GameObject[size];
    }
}

[RequireComponent(typeof(SimpleObjectPooler))]
public class ScenarySpawner : MonoBehaviour
{
    public static ScenarySpawner Instance = null;   

    [Header("Obstacle Blocks Prefabs for each Speed Stage")]
    public LGO[] StagePrefabsArray = { };

    [Header("Speed Stages %")]
    [Tooltip("Porcentagem da velocidade maxima na qual muda para o proximo estagio de blocos a serem spawnados.")]
    [HideInInspector] public float[] StageChangePercentages = { };
    [HideInInspector] public int CurrentStage = 0;
    [HideInInspector] public int StageNumber = 1;
    
    [Header("Initial Spawn Configurations")]
    [SerializeField] protected GameObject InitialPrefab = null;
    [SerializeField] protected int InitialPrefabNumber = 1;
    [SerializeField] protected float DistanceToPlayer = 20f;

    protected SimpleObjectPooler _pooler = null;
    protected Transform _lastTransform = null;
    protected Vector3 playerPosition = Vector3.zero;

    protected virtual void Awake()
    {
        // Prepare Find player position
        playerPosition = FindObjectOfType<PlayerMovement>().transform.position;
        
        // Get pooler reference
        _pooler = GetComponent<SimpleObjectPooler>();
        
        // Get initial end position
        _lastTransform = InitialPrefab.transform.Find("EndPosition");
    }

    public virtual void Initialize()
    {
        List<GameObject> Prefabs = new List<GameObject>();
        foreach (var l in StagePrefabsArray)
            Prefabs.AddRange(l.Array);

        _pooler.Initialization(Prefabs.ToArray());

        // Spawn Ahead Scenaries
        for (int i = 0; i < InitialPrefabNumber; i++)
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
        if (TimeToSpeedManager.instance.IsGamePaused) return false;

        return (Vector3.Distance(playerPosition, _lastTransform.position) < DistanceToPlayer * InitialPrefabNumber);
    }

    protected virtual void InstantiateScenary()
    {
        // Don't do nothing if game is paused
        if (TimeToSpeedManager.instance.IsGamePaused) return;

        // Check what stage is in
        CurrentStage = CheckCurrentStage();

        // Get random scenary from pool
        GameObject scenary = null;
        int tries = 0;
        while (scenary == null)
        {
            if (tries >= StagePrefabsArray[CurrentStage].Length)
            {
                Debug.Log($"No available object in pool of {gameObject.name}");
                return;
            }
            int random_index = UnityEngine.Random.Range((int)0, (int)StagePrefabsArray[CurrentStage].Length);
            scenary = _pooler.GetObject(StagePrefabsArray[CurrentStage][random_index]);
            if (scenary == null)
            {
                Debug.Log($"Couldnt find {StagePrefabsArray[CurrentStage][random_index].name} of index {random_index} from {gameObject.name}");
            }
            tries++;
        }

        PositionObject(scenary);
    }

    protected virtual void PositionObject(GameObject scenary)
    {
        // Update Last transform
        scenary.transform.position = _lastTransform.position;
        scenary.SetActive(true);

        // Update End Position
        _lastTransform = scenary.transform.Find("EndPosition");
    }

    protected virtual int CheckCurrentStage()
    {
        int stage = 0;
        float evaluatedSpeed = TimeToSpeedManager.instance.EvaluatedSpeed;

        foreach (var p in StageChangePercentages)
        {
            //0.3 , 0.7
            Debug.Log(p);
            if (evaluatedSpeed > p)
            {
                stage++;
                Debug.Log($"Stage Up {stage} for {evaluatedSpeed}");
            }
            else
            {
                return stage;
            }
        }
        return stage;
    }
}
