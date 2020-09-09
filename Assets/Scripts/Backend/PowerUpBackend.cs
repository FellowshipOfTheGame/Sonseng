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
    public struct PostResponse{
        public int cogs;
    }
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
        WWWForm form = new WWWForm();
        form.AddField("uid",UserBackend.instance.userId);
        form.AddField("powerUp", powerUp);
        StartCoroutine(RequestManager.PostRequest<PostResponse>("purchasePowerUp",form, FinishPurchase, LoadError));
    }

    public void LoadError(string errorMessage){
        Debug.Log(errorMessage);
    }
    public void FinishPurchase(PostResponse newCogs){
        Debug.Log(newCogs.cogs);
    }

}