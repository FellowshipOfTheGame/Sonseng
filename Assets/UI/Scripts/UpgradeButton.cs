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

    public TextMeshProUGUI iconTxt, costTxt;

    public string upgradeName;
    private bool hasRead = false;
    public PowerUpBackend backend;

    public void UpdateIcon() {
        switch (upgradeName) {
            case "double":
                iconTxt.text = doublePoints + "\uf067";
                break;
            case "speed":
                iconTxt.text = speed + "\uf067";
                break;
            case "magnet":
                iconTxt.text = magnet + "\uf067";
                break;
            case "shield":
                iconTxt.text = shield + "\uf067";
                break;
            case "p-button":
                iconTxt.text = pButton + "\uf067";
                break;
            case "invincibility":
                iconTxt.text = invincibility + "\uf067";
                break;
            default:
                break;
        }
    }

    public void UpdateCost() {
        costTxt.text = backend.prices[upgradeName].ToString();
    }

    public void DisableButton() {
        costTxt.text = "MAX";
        GetComponent<Image>().color = disabled;
        GetComponent<Button>().enabled = false;
    }

    void OnEnable() {
        hasRead = false;
    }

    void Update() {
        if (!hasRead && backend.finishedGettingPrice && UserBackend.instance.finishGetUpgrades) {
            if (UserBackend.instance.boughtUpgrades.ContainsKey(upgradeName)) {
                if (backend.prices[upgradeName].max) {
                    DisableButton();
                } else {
                    costTxt.text = backend.prices[upgradeName].price.ToString();
                    if (backend.prices[upgradeName].level >= 0) {
                        UpdateIcon();
                    }
                }
                hasRead = true;
            }
        }
    }

}