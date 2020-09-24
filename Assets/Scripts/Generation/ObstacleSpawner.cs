using System;
using System.Collections.Generic;
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
        Assert.IsNotNull(TimeToSpeedManager.instance, $"GameManager instance is null for {name}");
    # endif
    }

    protected override void PositionObject(GameObject scenary)
    {
        // Update Last transform
        float distance = MinDistance + SpeedToDistanceCurve.Evaluate(TimeToSpeedManager.instance.EvaluatedSpeed) * (MaxDistance - MinDistance);
        Vector3 offset = Vector3.forward * distance;
        scenary.transform.position = _lastTransform.position + offset;
        scenary.SetActive(true);

        // Update End Position
        _lastTransform = scenary.transform.Find("EndPosition");
    }
}
