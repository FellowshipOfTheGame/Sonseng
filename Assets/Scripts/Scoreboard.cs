using TMPro;
using UnityEngine;

public class Scoreboard : MonoBehaviour {
    [HideInInspector] public static Scoreboard instance;
    [SerializeField] TextMeshProUGUI scoreText, cogText, shopCogText, homeCogText;
    [SerializeField] float speed;

    [SerializeField] float defaultBonusScore;
    private bool isPlayerAlive = true;
    private float score;
    public float scoreMultiplier = 1f;

    [SerializeField] Transform feedbackPos;
    [SerializeField] GameObject bonusPointFeedback;

    private int cogs = 0;
    public int Cogs {
        get{ return cogs; }
        set{ cogs = value;}
    }
    public float highestScore;

    public int highestScoreRounded => (int) Mathf.Round(highestScore); 

    public void StopScore() {
        isPlayerAlive = false;
    }

    public float Score {
        get { return score; }
        private set {
            score = value;
            scoreText.text = ScoreRounded.ToString() + " pontos";
        }
    }

    public int ScoreRounded => (int) Mathf.Round(score);

    private void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

        Score = 0;
    }

    public void AddCog(){
        cogs++;
        cogText.text = cogs.ToString().PadLeft(3,'0');
    }
    /// <summary>
    /// Adds score proportionally to speed
    /// </summary>
    private void FixedUpdate() {
        if(isPlayerAlive) Score += TimeToSpeedManager.instance.EvaluatedSpeed * scoreMultiplier;
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
        bonusScore = Mathf.Round(bonusScore);
        Score += bonusScore;
        var feedback = Instantiate(bonusPointFeedback, feedbackPos.position, Quaternion.identity, feedbackPos);
        feedback.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "+" + bonusScore.ToString();
    }

    public void ResetScore() {
        Score = 0;
    }

    public void UpdateCogsText(int cogsRecv){
        homeCogText.text = cogsRecv.ToString();
        shopCogText.text = cogsRecv.ToString();
    }
}