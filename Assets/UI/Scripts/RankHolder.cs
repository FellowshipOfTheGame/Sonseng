using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankHolder : MonoBehaviour {
    public string playerName, score, rank;

    [SerializeField]
    private TextMeshProUGUI rankText, nameTxt, scoreTxt;

    [SerializeField]
    private Color firstTextColor, firstImageColor;
    
    [SerializeField]
    private Image background;
    void Start() {
        if(int.Parse(rank) == 1){
            rankText.color = firstTextColor;
            background.color = firstImageColor;
        }
        rankText.text = rank + "º";
        nameTxt.text = playerName;
        scoreTxt.text = score + " metros";
    }
}