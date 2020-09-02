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

    private void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);
    }

    void Start() {
#if UNITY_EDITOR
        Firebase.FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://sonseng2020-1586957105557.firebaseio.com/");
#endif
        database = FirebaseDatabase.DefaultInstance;
        reference = database.RootReference;
        user = FirebaseAuth.DefaultInstance.CurrentUser;
        userId = user.UserId;
        GetCogs();
    }

    public void GetCogs() {
        reference.Child($"/users/{userId}/coins").GetValueAsync().ContinueWith(task => {
            if (task.IsCompleted) {
                DataSnapshot data = task.Result;
                cogs = int.Parse(data.GetRawJsonValue());
            }
        });
    }
}