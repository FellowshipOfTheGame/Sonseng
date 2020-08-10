using UnityEngine;
using TMPro;

public class Scoreboard : MonoBehaviour {
    [HideInInspector] public Scoreboard instance;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] float speed;
    [SerializeField] float defaultBonusScore;
    private float score;

    private float Score {
        get { return score; }
        set {
            score = value;
            scoreText.text = "Score: " + Mathf.Round(score).ToString();
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
        Score += speed;
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