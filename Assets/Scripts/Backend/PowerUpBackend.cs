using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpBackend : MonoBehaviour {

    private UpgradeButton upgradeButton;
    public GameObject buttonClicked;
    private bool beingClicked = false;
    public Dictionary<string, int> prices = new Dictionary<string, int>() { { "double", -1 }, { "shield", -1 }, { "magnet", -1 }, { "p-button", -1 }, { "invincibility", -1 }, { "speed", -1 },
    };
    public struct PurchaseResponse {
        public int cogs;
        public int nextPrice;
        public string powerUp;
    }

    public struct PriceResponse {
        public string powerUp;
        public int price;

    }

    [SerializeField] private TextMeshProUGUI cogsText;

    void Start() {
        cogsText.text = UserBackend.instance.cogs.ToString();
    }

    public void BuyPowerUp(string powerUp) {
        if (!beingClicked)
            StartCoroutine(BuyPowerUpCourotine(powerUp));
    }

    private IEnumerator BuyPowerUpCourotine(string powerUp) {
        beingClicked = true;
        WWWForm form = new WWWForm();
        form.AddField("uid", UserBackend.instance.userId);
        form.AddField("powerUp", powerUp);
        buttonClicked = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        yield return StartCoroutine(RequestManager.PostRequest<PurchaseResponse>("powerup/purchasePowerUp", form, FinishPurchasePowerUp, LoadErrorPurchase));
        beingClicked = false;
    }

    public void BuyUpgrade(string powerUp) {
        if (!beingClicked)
            StartCoroutine(BuyUpgradeCourotine(powerUp));
    }

    private IEnumerator BuyUpgradeCourotine(string powerUp) {
        beingClicked = true;
        WWWForm form = new WWWForm();
        form.AddField("uid", UserBackend.instance.userId);
        form.AddField("powerUp", powerUp);
        buttonClicked = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        yield return StartCoroutine(RequestManager.PostRequest<PurchaseResponse>("powerup/purchaseUpgrade", form, FinishPurchaseUpgrade, LoadErrorPurchase));
        beingClicked = false;
    }

    public void LoadErrorPrice(string errorMessage) {
        Debug.Log(errorMessage);
    }
    public void LoadErrorPurchase(string errorMessage) {
        if (errorMessage == "Você não tem engrenagens suficientes!") {} else {
            Regex rx = new Regex(@"(?<=\!).+?(?=\!)");
            MatchCollection match = rx.Matches(errorMessage);
            Debug.Log(match[0].Value);
            prices[match[0].Value] = -1;
            buttonClicked.GetComponent<UpgradeButton>().DisableButton();
            UserBackend.instance.GetCogs();
        }
    }

    public void FinishPurchasePowerUp(PurchaseResponse res) {
        UpgradeButton currentButton = buttonClicked.GetComponent<UpgradeButton>();
        UserBackend.instance.GetBoughtUpgrades();
        currentButton.UpdateIcon();

        buttonClicked.GetComponent<Button>().onClick.RemoveAllListeners();
        buttonClicked.GetComponent<Button>().onClick.AddListener(delegate() {
            BuyUpgrade(currentButton.upgradeName);
        });

        UserBackend.instance.cogs = res.cogs;
        cogsText.text = res.cogs.ToString();
        prices[res.powerUp] = res.nextPrice;
        currentButton.costTxt.text = res.nextPrice.ToString();
    }

    public void FinishPurchaseUpgrade(PurchaseResponse res) {
        UpgradeButton currentButton = buttonClicked.GetComponent<UpgradeButton>();
        UserBackend.instance.GetBoughtUpgrades();
        currentButton.UpdateIcon();
        UserBackend.instance.cogs = res.cogs;
        prices[res.powerUp] = res.nextPrice;
        cogsText.text = res.cogs.ToString();
        buttonClicked.GetComponent<UpgradeButton>().costTxt.text = res.nextPrice.ToString();
    }

    public IEnumerator GetCurrentPrice(string powerUp) {
        WWWForm form = new WWWForm();
        form.AddField("uid", UserBackend.instance.userId);
        form.AddField("powerUp", powerUp);
        yield return StartCoroutine(RequestManager.PostRequest<PriceResponse>("powerup/getCurrentPrice", form, FinishGetPrice, LoadErrorPrice));

    }

    public void FinishGetPrice(PriceResponse priceRes) {
        prices[priceRes.powerUp] = priceRes.price;
    }

}