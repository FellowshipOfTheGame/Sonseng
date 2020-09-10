using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;

public class FirebaseInitializer : MonoBehaviour {
    [HideInInspector] public static FirebaseInitializer instance;
    public FirebaseDatabase database;
    public FirebaseUser user;
    public DatabaseReference reference;

    private void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);
#if UNITY_EDITOR
        Firebase.FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://sonseng2020-1586957105557.firebaseio.com/");
#endif
        database = FirebaseDatabase.DefaultInstance;
        reference = database.RootReference;
        user = FirebaseAuth.DefaultInstance.CurrentUser;

    }

}