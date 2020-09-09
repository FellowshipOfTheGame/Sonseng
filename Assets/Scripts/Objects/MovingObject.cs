using UnityEngine.Assertions;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
#if UNITY_EDITOR
    private void Awake()
    {
        Assert.IsNotNull(GameManager.instance);
    }
#endif

    // Update is called once per frame
    void FixedUpdate()
    {
      
        transform.position += -transform.forward * (Time.deltaTime * GameManager.instance.Speed);
    }
}
