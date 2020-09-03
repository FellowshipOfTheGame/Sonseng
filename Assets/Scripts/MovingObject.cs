using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    // Update is called once per frame
    void FixedUpdate()
    {
        if(ScenarySpawner.Instance.spawnActive)
            transform.position += -transform.forward * (Time.deltaTime * BlocksGenerator.Instance.Speed);
    }
}
