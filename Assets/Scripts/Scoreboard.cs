using TMPro;
using UnityEngine;

public class Scoreboard : MonoBehaviour {
    [HideInInspector] public static Scoreboard instance;
    [SerializeField] TextMeshProUGUI scoreText, cogText, shopCogText, homeCogText;
    [SerializeField] float speed;
    [SerializeField] float defaultBonusScore;
    private bool isPlayerAlive = true;
    private float score;
    private int cogs = 0;
    public int Cogs {
        get{ return cogs; }
        set{ cogs = value;}
    }
    public float highestScore;

    public void StopScore() {
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

    public void AddCog(){
        cogs++;
        cogText.text = cogs.ToString().PadLeft(3,'0');
    }
    /// <summary>
    /// Adds score proportionally to speed
    /// </summary>
    private void FixedUpdate() {
        if (isPlayerAlive)Score += speed;
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

    public void UpdateCogsText(int cogsRecv){
        homeCogText.text = cogsRecv.ToString();
        shopCogText.text = cogsRecv.ToString();
    }
}