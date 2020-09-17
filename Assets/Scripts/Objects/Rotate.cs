using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {
    [SerializeField] float rotationSpeed;
    void Update() {
        this.transform.eulerAngles += new Vector3(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
