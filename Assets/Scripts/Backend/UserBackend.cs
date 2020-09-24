using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using TMPro;
using UnityEngine;

public class UserBackend : MonoBehaviour {
    [HideInInspector] public static UserBackend instance;
    public int cogs;
    private FirebaseDatabase database;
    private DatabaseReference reference;
    public string userId;
    [Serializable]
    public struct Upgrade {
        public string upgradeName;
        public float baseValue;
        public int level;
        public float multiplier;
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
#if UNITY_EDITOR
        Firebase.FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://sonseng2020-1586957105557.firebaseio.com/");
#endif
        database = FirebaseDatabase.DefaultInstance;
        reference = database.RootReference;
    } 
 
    public void UpdateUserReference() {
        userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        GetCogs();
        GetBoughtUpgrades();
    }
    public void GetCogs() {
        reference.Child($"/users/{userId}/coins").GetValueAsync().ContinueWith(task => {
            if (task.IsCompleted) {
                DataSnapshot data = task.Result;
                cogs = int.Parse(data.GetRawJsonValue());
            }
        });

    }

    public void GetBoughtUpgrades() {
        StartCoroutine(GetBoughtUpgradesCourotine());
    }

    private IEnumerator GetBoughtUpgradesCourotine() {
        WWWForm form = new WWWForm();
        form.AddField("uid", userId);
        yield return StartCoroutine(RequestManager.PostRequest<UpgradeRoot>("user/getBoughtUpgrades", form, FinishGetBoughtUpgrades, LoadError));
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