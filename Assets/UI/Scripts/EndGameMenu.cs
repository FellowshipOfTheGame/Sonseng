using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGameMenu : MonoBehaviour {
    [SerializeField] TextMeshProUGUI scoreText, recordText;

    void OnEnable() {
        if(Scoreboard.instance.Score > Scoreboard.instance.highestScore){
            Scoreboard.instance.highestScore = Scoreboard.instance.Score;
        }
        scoreText.text = Scoreboard.instance.Score + " metros";
        recordText.text = Scoreboard.instance.highestScore.ToString();
     }
}