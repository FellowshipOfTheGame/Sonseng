using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGameMenu : MonoBehaviour {
    [SerializeField] TextMeshProUGUI scoreText, recordText, cogsText;

    void OnEnable() {
        if(Scoreboard.instance.Score > Scoreboard.instance.highestScore){
            Scoreboard.instance.highestScore = Scoreboard.instance.Score;
        }
        scoreText.text = Scoreboard.instance.ScoreRounded + " Pontos";
        recordText.text = Scoreboard.instance.highestScoreRounded.ToString();
        cogsText.text = Scoreboard.instance.Cogs.ToString().PadLeft(3 , '0');
     }
}