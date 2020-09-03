using UnityEngine;
using TMPro;

public class Scoreboard : MonoBehaviour {
    [HideInInspector] public static Scoreboard instance;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] float speed;
    [SerializeField] float defaultBonusScore;
    private bool isPlayerAlive = true;
    private float score;

    public float highestScore;

    public void StopScore(){
        isPlayerAlive = false;
    }

    public float Score {
        get { return score; }
        private set {
            score = value;
            scoreText.text = Mathf.Round(score).ToString() + " metros";
        }
    }

    private void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

        Score = 0;
    }

    /// <summary>
    /// Adds score proportionally to speed
    /// </summary>
    private void FixedUpdate() {
        if(isPlayerAlive) Score += speed;
    }

    /// <summary>
    /// Adds a default bonus to score
    /// </summary>
    public void AddBonus() {
        Score += defaultBonusScore;
    }

    /// <summary>
    /// Adds a specified bonus to score
    /// </summary>
    /// <param name="bonusScore">Value of the bonus that will be added to score</param>
    public void AddBonus(float bonusScore) {
        Score += bonusScore;
    }

    public void ResetScore() {
        Score = 0;
    }
}