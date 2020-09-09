using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class UpgradeButton : MonoBehaviour {
    public TextMeshProUGUI iconTxt, costTxt;

    public string cost, iconString;

    public string upgradeName;
    void OnEnable() {
        if (UserBackend.instance.boughtUpgrades.ContainsKey(upgradeName)) {
            iconTxt.text = iconString + "\uf067";
            iconTxt.ForceMeshUpdate();
            iconTxt.UpdateFontAsset();
            iconTxt.UpdateMeshPadding();
        } else {
            iconTxt.text = iconString;
            iconTxt.ForceMeshUpdate();
            iconTxt.UpdateFontAsset();
            iconTxt.UpdateMeshPadding();
        }
    }
}