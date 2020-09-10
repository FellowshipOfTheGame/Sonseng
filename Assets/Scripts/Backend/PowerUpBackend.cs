using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PowerUpBackend : MonoBehaviour {

    private string userId;
    private FirebaseDatabase database;
    private DatabaseReference reference;
    private UpgradeButton upgradeButton;
    public GameObject buttonClicked;

    public Dictionary<string, int> prices = new Dictionary<string, int>();
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
        database = FirebaseInitializer.instance.database;
        reference = FirebaseInitializer.instance.reference;
        cogsText.text = UserBackend.instance.cogs.ToString();
    }

    public void BuyPowerUp(string powerUp) {
        WWWForm form = new WWWForm();
        form.AddField("uid", UserBackend.instance.userId);
        form.AddField("powerUp", powerUp);
        buttonClicked = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        StartCoroutine(RequestManager.PostRequest<PurchaseResponse>("purchasePowerUp", form, FinishPurchasePowerUp, LoadError));
    }

    public void BuyUpgrade(string powerUp) {
        WWWForm form = new WWWForm();
        form.AddField("uid", UserBackend.instance.userId);
        form.AddField("powerUp", powerUp);
        buttonClicked = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        StartCoroutine(RequestManager.PostRequest<PurchaseResponse>("purchaseUpgrade", form, FinishPurchaseUpgrade, LoadError));

    }
    public void LoadError(string errorMessage) {
        Debug.Log(errorMessage);
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
        UserBackend.instance.cogs = res.cogs;
        prices[res.powerUp] = res.nextPrice;
        cogsText.text = res.cogs.ToString();
        buttonClicked.GetComponent<UpgradeButton>().costTxt.text = res.nextPrice.ToString();
    }

    public IEnumerator GetCurrentPrice(string powerUp) {
        WWWForm form = new WWWForm();
        form.AddField("uid", UserBackend.instance.userId);
        form.AddField("powerUp", powerUp);
        yield return StartCoroutine(RequestManager.PostRequest<PriceResponse>("getCurrentPrice", form, FinishGetPrice, LoadError));

    }

    public void FinishGetPrice(PriceResponse priceRes) {
        prices.Add(priceRes.powerUp, priceRes.price);
    }

}