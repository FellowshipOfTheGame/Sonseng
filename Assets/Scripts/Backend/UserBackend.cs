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
    public bool finishGetUpgrades = false;
    public struct Upgrade {
        public string upgradeName;
        public float baseValue;
        public int level;
        public float multiplier;
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
        finishGetUpgrades = false;
        boughtUpgrades = new Dictionary<string, Upgrade>();
        reference.Child($"/users/{userId}/bought-powerUps/").GetValueAsync().ContinueWith(task => {
            if (task.IsCompleted) {
                DataSnapshot data = task.Result;
                var upgrades = data.Value as Dictionary<string, object>;
                foreach (var upgrade in upgrades) {
                    Upgrade up = new Upgrade();
                    up.upgradeName = upgrade.Key;
                    var infos = upgrade.Value as Dictionary<string, object>;
                    foreach (var info in infos) {
                        if (info.Key == "level") {
                            up.level = int.Parse(info.Value.ToString());
                        } else if (info.Key == "multiplier") {
                            up.multiplier = float.Parse(info.Value.ToString());
                        } else {
                            up.baseValue = float.Parse(info.Value.ToString());
                        }
                    }
                    boughtUpgrades.Add(up.upgradeName, up);
                    RandomCollectableSystem.Instance.AddCollectable(up.upgradeName);
                }
                finishGetUpgrades = true;
            } else if (task.IsFaulted) {
                Debug.Log(task.Exception);
            }
        });
    }

}