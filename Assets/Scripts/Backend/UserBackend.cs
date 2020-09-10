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
    private FirebaseUser user;
    private FirebaseDatabase database;
    private DatabaseReference reference;
    public string userId;

    public Dictionary<string, Upgrade> boughtUpgrades = new Dictionary<string, Upgrade>();

    private void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);
        database = FirebaseInitializer.instance.database;
        reference = FirebaseInitializer.instance.reference;
        user = FirebaseInitializer.instance.user;
        userId = user.UserId;
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
                        } else {
                            up.multiplier = float.Parse(info.Value.ToString());
                        }
                    }
                    boughtUpgrades.Add(up.upgradeName, up);
                }

            } else if (task.IsFaulted) {
                Debug.Log(task.Exception);
            }
        });
    }

    public void AddCogs(int newCogs) {
        GetCogs();
        reference.Child($"/users/{userId}/coins").SetValueAsync(cogs + newCogs);
    }
}