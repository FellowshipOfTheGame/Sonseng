using System;
using UnityEngine;

public class ScoreBackend : MonoBehaviour {
    [SerializeField] bool debugMode;

    public struct ScoreResponse {
        public int cogs;
        public float highestScore;
    }

    [Serializable]
    public struct HighestScore{
        public int highestScore;
    }

    void Start() {
        GetHighestScore();
    }

    public void SaveScoreOnDeath() {
        Scoreboard.instance.StopScore();
        WWWForm form = new WWWForm();
        form.AddField("uid", UserBackend.instance.userId);
        form.AddField("score", Scoreboard.instance.ScoreRounded);
        form.AddField("cogs", Scoreboard.instance.Cogs);
        StartCoroutine(RequestManager.PostRequest<ScoreResponse>("user/saveStats", form, FinishSaveScore, LoadError));
    }

    public void FinishSaveScore(ScoreResponse res) {
        Scoreboard.instance.highestScore = res.highestScore;
        Scoreboard.instance.Cogs = res.cogs;
    }

    public void LoadError(string errorMessage) {
        Debug.Log(errorMessage);
    }
    public void GetHighestScore() {
        WWWForm form = new WWWForm();
        form.AddField("uid", UserBackend.instance.userId);
        StartCoroutine(RequestManager.PostRequest<HighestScore>("user/cogs", form, FinishGetHighScore, LoadError));
    }

    public void FinishGetHighScore(HighestScore highestScore) {
        Scoreboard.instance.highestScore = highestScore.highestScore;
    }
}