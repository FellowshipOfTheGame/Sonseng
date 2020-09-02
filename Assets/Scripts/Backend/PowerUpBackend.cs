using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using TMPro;
using UnityEngine;

public class PowerUpBackend : MonoBehaviour {

    private string userId;
    private FirebaseDatabase database;
    private DatabaseReference reference;

    [SerializeField] private TextMeshProUGUI cogsText;

    void Start() {
#if UNITY_EDITOR
        Firebase.FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://sonseng2020-1586957105557.firebaseio.com/");
#endif
        database = FirebaseDatabase.DefaultInstance;
        reference = database.RootReference;
        cogsText.text = UserBackend.instance.cogs.ToString();

    }

    public void BuyPowerUp(string powerUp) {
        reference.Child($"users/{UserBackend.instance.userId}/bought-powerUps/{powerUp}").SetValueAsync(true);
        UserBackend.instance.GetCogs();
        cogsText.text = UserBackend.instance.cogs.ToString();
    }

}