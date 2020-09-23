using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UpgradeButton : MonoBehaviour {
    private const string doublePoints = "x2";
    private const string speed = "\uf3fd";
    private const string magnet = "\uf076";
    private const string shield = "\uf004";
    private const string invincibility = "\uf005";
    private const string pButton = "\uf552";
    public Color disabled;

    public TextMeshProUGUI iconTxt, buttonText, costText;
    [SerializeField]
    private string upgradeName;
    [SerializeField]
    private string unidade;
    private bool hasRead = false;
    public PowerUpBackend backend;

    [SerializeField] private string description;
    [SerializeField] private TextMeshProUGUI descText;

    //private float baseValue, previousMult, nextMult;

    void Start() {
        descText.text = description;
    }
    public void UpdateInfos() {
        PowerUpBackend.PriceResponse infos = backend.prices[upgradeName];
        buttonText.text = "MELHORAR\n" + string.Format("{0:0.00}", infos.baseValue * infos.prevMult) + unidade + " " + "\uf061" + string.Format("{0:0.00}", infos.baseValue * infos.nextMult) + unidade;
        costText.text = infos.price.ToString() + " \uf013";
    }

    public void DisableButton() {
        costText.text = "MAX";
        buttonText.text = "NÍVEL MÁXIMO!";
        GetComponent<Image>().color = disabled;
        GetComponent<Button>().enabled = false;
    }

    void OnEnable() {
        hasRead = false;
    }

    void Update() {
        if (!hasRead && backend.finishedGettingPrice) {

            if (backend.prices[upgradeName].max) {
                DisableButton();
            } else {
                if (backend.prices[upgradeName].level >= 0) {
                    UpdateInfos();
                } else {
                    costText.text = backend.prices[upgradeName].price.ToString() + " \uf013";
                }
            }
            hasRead = true;

        }
    }

}