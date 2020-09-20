using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeToSpeedManager : MonoBehaviour
{
    [HideInInspector] public static TimeToSpeedManager instance = null;

    [SerializeField] protected AnimationCurve TimeToSpeedCurve = null;

    /// <summary>
    /// Use StopGame() to pause the objects spawn and IsGamePaused to check game pause condition. 
    /// </summary>
    [SerializeField] public bool IsGamePaused { get => _gamePaused; }

    private bool _gamePaused = false;

    [Header("Speed Properties")]
    [Tooltip("Values for minimum and maximum speed in a run")]
    [SerializeField] protected float MaxSpeed = 1f;
    [SerializeField] protected float MinSpeed = 0f;

    [Header("Time Properties")]
    [Tooltip("Time, in seconds, taken to reach Max Speed since start of the run/game")]
    [SerializeField] protected float TimeForMaxSpeed = 1f;

    protected float _currentTime = 0f;
    /// <summary>
    /// Get Time since game started in seconds
    /// </summary>
    public float TimeSinceGameStarted => _currentTime;

    /// <summary>
    /// Get Game Speed in interval [0,1]
    /// </summary>
    public float EvaluatedSpeed => _gamePaused ? 0f : TimeToSpeedCurve.Evaluate(Mathf.Min(TimeSinceGameStarted / TimeForMaxSpeed, TimeForMaxSpeed));

    /// <summary>
    /// Get Actual Game Speed
    /// </summary>
    public float Speed => _gamePaused ? 0f : MinSpeed + EvaluatedSpeed * (MaxSpeed - MinSpeed);


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

        // StartNewGame();
    }

    /// <summary>
    /// Start a new game
    /// Resets game elapsed time
    /// </summary>
    public void StartNewGame()
    {
        _currentTime = 0f;
        ResumeGame();
    }

    /// <summary>
    /// Pauses the game (object spawning)
    /// </summary>
    public void StopGame()
    {
        //Time.timeScale = 0f;
        _gamePaused = true;
    }

    /// <summary>
    /// Resumes the game (object spawning)
    /// </summary>
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        _gamePaused = false;
    }

    private void Update()
    {
        if (_gamePaused)
            return;

        _currentTime += Time.deltaTime;
    }
}
