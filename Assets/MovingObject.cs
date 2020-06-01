using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    private void Awake()
    {
        Invoke("Kill", 5f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += -transform.forward * (Time.deltaTime * RandomObjectGenerator.Instance.Speed);
    }

    void Kill()
    {
        Destroy(gameObject);
    }
}
