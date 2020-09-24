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
    public GameObject buttonClicked, errorPanel;
    private bool beingClicked = false;
    public bool finishedGettingPrice = false;
    public Dictionary<string, PriceResponse> prices = new Dictionary<string, PriceResponse>();
    public struct PurchaseResponse {
        public int cogs;
        public int nextPrice;
        public string powerUp;

        public float prevMult;
        public float nextMult;

    }

    [Serializable]
    public struct PriceResponse {
        public string name;
        public int price;
        public bool max;
        public int level;
        public float baseValue;
        public float prevMult;
        public float nextMult;

    }

    [Serializable]
    public struct PricesRoot {
        public PriceResponse[] prices;
    }

    [SerializeField] private TextMeshProUGUI cogsText;

    void Start() {
        if (Scoreboard.instance != null) {

            Scoreboard.instance.UpdateCogsText(Scoreboard.instance.Cogs);
        } else {
            cogsText.text = UserBackend.instance.cogs.ToString();
        }
    }

    void OnEnable() {
        finishedGettingPrice = false;
        StartCoroutine(GetAllPrices());
    }
    public void BuyPowerUp(string powerUp) {
        if (!beingClicked)
            StartCoroutine(BuyPowerUpCourotine(powerUp));
    }

    private IEnumerator BuyPowerUpCourotine(string powerUp) {
        LoadingCircle.instance.EnableOrDisable(true);
        beingClicked = true;
        WWWForm form = new WWWForm();
        form.AddField("uid", UserBackend.instance.userId);
        form.AddField("powerUp", powerUp);
        buttonClicked = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        if (prices[powerUp].level == -1) {
            yield return StartCoroutine(RequestManager.PostRequest<PurchaseResponse>("powerup/purchasePowerUp", form, FinishPurchasePowerUp, LoadErrorPurchase));
            beingClicked = false;
            LoadingCircle.instance.EnableOrDisable(false);
        } else {
            yield return StartCoroutine(RequestManager.PostRequest<PurchaseResponse>("powerup/purchaseUpgrade", form, FinishPurchaseUpgrade, LoadErrorPurchase));
            beingClicked = false;
            LoadingCircle.instance.EnableOrDisable(false);
        }
    }

    public void LoadErrorPrice(string errorMessage) {
        Debug.Log(errorMessage);
    }
    public void LoadErrorPurchase(string errorMessage) {
        if (errorMessage == "Você não tem engrenagens suficientes!") {
            errorPanel.SetActive(true);
        }
    }

    public void FinishPurchasePowerUp(PurchaseResponse res) {
        UpgradeButton currentButton = buttonClicked.GetComponent<UpgradeButton>();
        UserBackend.instance.GetBoughtUpgrades();
        UserBackend.instance.cogs = res.cogs;
        if (Scoreboard.instance != null)
            Scoreboard.instance.UpdateCogsText(res.cogs);
        else
            cogsText.text = res.cogs.ToString();
        PriceResponse temp = prices[res.powerUp];
        temp.price = res.nextPrice;
        temp.level++;
        temp.prevMult = res.prevMult;
        temp.nextMult = res.nextMult;
        prices[res.powerUp] = temp;
        currentButton.UpdateInfos();
        //currentButton.costTxt.text = res.nextPrice.ToString();
        RandomCollectableSystem.Instance.AddCollectable(res.powerUp);
    }

    public void FinishPurchaseUpgrade(PurchaseResponse res) {
        UpgradeButton currentButton = buttonClicked.GetComponent<UpgradeButton>();
        UserBackend.instance.GetBoughtUpgrades();
        UserBackend.instance.cogs = res.cogs;
        if (Scoreboard.instance != null)
            Scoreboard.instance.UpdateCogsText(res.cogs);
        else
            cogsText.text = res.cogs.ToString();
        PriceResponse temp = prices[res.powerUp];
        if (res.nextPrice == -1) {
            currentButton.DisableButton();
        } else {
            temp.price = res.nextPrice;
            temp.level++;
            temp.prevMult = res.prevMult;
            temp.nextMult = res.nextMult;
            prices[res.powerUp] = temp;
            currentButton.UpdateInfos();
            cogsText.text = res.cogs.ToString();
            //buttonClicked.GetComponent<UpgradeButton>().costTxt.text = res.nextPrice.ToString();
        }
    }

    public IEnumerator GetAllPrices() {
        LoadingCircle.instance.EnableOrDisable(true);
        finishedGettingPrice = false;
        WWWForm form = new WWWForm();
        form.AddField("uid", UserBackend.instance.userId);
        yield return StartCoroutine(RequestManager.PostRequest<PricesRoot>("powerup/getAllPrices", form, FinishGetAllPrices, LoadErrorPrice));
        LoadingCircle.instance.EnableOrDisable(false);
    }

    public void FinishGetAllPrices(PricesRoot root) {
        foreach (var price in root.prices) {
            if (!prices.ContainsKey(price.name)) {
                prices.Add(price.name, price);
            }
        }
        finishedGettingPrice = true;
    }

}