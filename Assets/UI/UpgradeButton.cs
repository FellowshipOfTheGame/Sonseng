using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class UpgradeButton : MonoBehaviour {
    public TextMeshProUGUI costTxt;

    public string cost;

    void Start() {
        costTxt.text = cost;
    }

    public void HandleClick(){
        Debug.Log("Ha");
    }
}