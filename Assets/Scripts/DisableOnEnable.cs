using UnityEngine;

public class DisableOnEnable : MonoBehaviour
{
    void OnEnable()
    {
        this.gameObject.SetActive(false);
    }
}