using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class ScoreBackend : MonoBehaviour {
    [SerializeField] bool debugMode;

    public struct ScoreResponse {
        public int cogs;
        public float highestScore;
    }

    [Serializable]
    public struct HighestScore {
        public int highestScore;
    }

    void Start() {
        GetHighestScore();
    }

    private string Hash() {
        Regex rx = new Regex(@"\.(.*?)\.");
        Match match = rx.Match(RequestManager.token);
        SHA256 sha = new SHA256CryptoServiceProvider();
        byte[] bytes = Encoding.ASCII.GetBytes("saiHackerFedido" + Scoreboard.instance.ScoreRounded.ToString() + Scoreboard.instance.highestScore.ToString() + Scoreboard.instance.Cogs.ToString() + match.Groups[0]);
        byte[] hashBytes = sha.ComputeHash(bytes);
        string hash = ByteArrayToString(hashBytes);
        return hash;
    }
    private static string ByteArrayToString(byte[] ba) {
        StringBuilder hex = new StringBuilder(ba.Length * 2);
        foreach (byte b in ba)
            hex.AppendFormat("{0:x2}", b);
        return hex.ToString();
    }
    public void SaveScoreOnDeath() {
        Scoreboard.instance.StopScore();
        WWWForm form = new WWWForm();
        form.AddField("score", Scoreboard.instance.ScoreRounded.ToString());
        form.AddField("highestScore", Scoreboard.instance.highestScore.ToString());
        form.AddField("cogs", Scoreboard.instance.Cogs.ToString());
        form.AddField("hash", Hash());
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
        StartCoroutine(RequestManager.GetRequest<HighestScore>("user/cogs", FinishGetHighScore, LoadError));
    }

    public void FinishGetHighScore(HighestScore highestScore) {
        Scoreboard.instance.highestScore = highestScore.highestScore;
    }
}