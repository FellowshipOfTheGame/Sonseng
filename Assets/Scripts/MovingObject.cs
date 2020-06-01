using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += -transform.forward * (Time.deltaTime * RandomObjectGenerator.Instance.Speed);
    }
}
