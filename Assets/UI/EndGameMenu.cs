using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGameMenu : MonoBehaviour {
    [SerializeField] TextMeshProUGUI scoreText, recordText;
    [SerializeField] private Scoreboard scoreboard;

    void OnEnable() {
        if(scoreboard.instance.Score > scoreboard.instance.highestScore){
            scoreboard.instance.highestScore = scoreboard.instance.Score;
        }
        scoreText.text = scoreboard.instance.Score + " metros";
        recordText.text = scoreboard.instance.highestScore.ToString();

    }
}