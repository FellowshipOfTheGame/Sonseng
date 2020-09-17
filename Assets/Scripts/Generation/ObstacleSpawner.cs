using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(SimpleObjectPooler))]
public class ObstacleSpawner : ScenarySpawner
{
    [Header("Distance From Obstacles Properties")]
    [Tooltip("Used for calculating the distance between obstacles based on the current game speed")]
    [SerializeField] protected AnimationCurve SpeedToDistanceCurve = null;
    [Tooltip("Maximum distance between obstacles")]
    [SerializeField] protected float MaxDistance = 1f;
    [Tooltip("Minimum distance between obstacles")]
    [SerializeField] protected float MinDistance = 0f;

    protected override void Awake()
    {
        base.Awake();
    #if UNITY_EDITOR
        Assert.IsNotNull(GameManager.instance, $"GameManager instance is null for {name}");
    # endif
    }

    protected override void InstantiateScenary()
    {
        // Don't do nothing if game is paused
        if (GameManager.instance.IsGamePaused) return;

        // Get random scenary from pool
        GameObject scenary = null;
        int tries = 0;
        while (scenary == null)
        {
            if (tries >= Prefabs.Length)
            {
                Debug.Log($"No available object in pool of {gameObject.name}");
                return;
            }
            int random_index = UnityEngine.Random.Range((int)0, (int)Prefabs.Length);
            scenary = _pooler.GetObject(Prefabs[random_index]);
            if (scenary == null)
            {
                Debug.Log($"Couldnt find object {Prefabs[random_index].name} of index {random_index} from {gameObject.name}");
            }
            tries++;
        }

        // Update Last transform
        float distance = MinDistance + SpeedToDistanceCurve.Evaluate(GameManager.instance.EvaluatedSpeed) * (MaxDistance - MinDistance);
        Vector3 offset = Vector3.forward * distance;
        scenary.transform.position = _lastTransform.position + offset;
        scenary.SetActive(true);

        // Update End Position
        _lastTransform = scenary.transform.Find("EndPosition");
    }
}
