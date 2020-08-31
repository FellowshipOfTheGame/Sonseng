using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager instance = null;

    [SerializeField] protected AnimationCurve TimeToSpeedCurve = null;

    [Header("Speed Properties")]
    [Tooltip("Values for minimum and maximum speed in a run")]
    [SerializeField] protected float MaxSpeed = 1f;
    [SerializeField] protected float MinSpeed = 0f;

    [Header("Time Properties")]
    [Tooltip("Time, in seconds, taken to reach Max Speed since start of the run/game")]
    [SerializeField] protected float TimeForMaxSpeed = 1f;

    protected float _timeThatGameStarted = 0f;
    /// <summary>
    /// Get Time since game started in seconds
    /// </summary>
    public float TimeSinceGameStarted => Time.timeSinceLevelLoad - _timeThatGameStarted;

    /// <summary>
    /// Get Game Speed in interval [0,1]
    /// </summary>
    public float EvaluatedSpeed => TimeToSpeedCurve.Evaluate(Mathf.Min(TimeSinceGameStarted / TimeForMaxSpeed, TimeForMaxSpeed));

    /// <summary>
    /// Get Actual Game Speed
    /// </summary>
    public float Speed => MinSpeed + EvaluatedSpeed * (MaxSpeed - MinSpeed);


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);
        StartNewGame();
    }

    /// <summary>
    /// Start a new game
    /// Resets game elapsed time
    /// </summary>
    public void StartNewGame()
    {
        _timeThatGameStarted = Time.timeSinceLevelLoad;
    }
}
