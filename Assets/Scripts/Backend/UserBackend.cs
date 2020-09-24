using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR
using Firebase.Auth;
#endif
public class UserBackend : MonoBehaviour {
    [HideInInspector] public static UserBackend instance;
    public int cogs;
    public string userId;
    [Serializable]
    public struct Upgrade {
        public string upgradeName;
        public float baseValue;
        public int level;
        public float multiplier;
    }

    [Serializable]
    public struct Cogs{
        public int cogs;
    }

    [Serializable]
    public struct UpgradeRoot {
        public Upgrade[] powerUps;
    }
    public Dictionary<string, Upgrade> boughtUpgrades = new Dictionary<string, Upgrade>();

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(instance);
        } else if (instance != this)
            Destroy(this.gameObject);

    }

    public void UpdateUserReference() {
#if UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR
        userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
#endif

        GetCogs();
        GetBoughtUpgrades();
    }
    public void GetCogs() {
        StartCoroutine(RequestManager.GetRequest<Cogs>("user/cogs", FinishGetCogs, LoadError));
    }

    private void FinishGetCogs(Cogs cogs) {
        instance.cogs = cogs.cogs;
    }

    public void GetBoughtUpgrades() {
        StartCoroutine(GetBoughtUpgradesCourotine());
    }

    private IEnumerator GetBoughtUpgradesCourotine() {
        yield return StartCoroutine(RequestManager.GetRequest<UpgradeRoot>("user/getBoughtUpgrades", FinishGetBoughtUpgrades, LoadError));
    }

    public void FinishGetBoughtUpgrades(UpgradeRoot root) {
        boughtUpgrades = new Dictionary<string, Upgrade>();
        foreach (Upgrade up in root.powerUps) {
            boughtUpgrades.Add(up.upgradeName, up);
            PowerUps.instance.SetPowerUpValue(up);
            RandomCollectableSystem.Instance.AddCollectable(up.upgradeName);
        }
    }

    public void LoadError(string errorMessage) {
        Debug.LogError(errorMessage);
    }

}