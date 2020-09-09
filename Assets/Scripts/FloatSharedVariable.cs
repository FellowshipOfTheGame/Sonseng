using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "sharedFloat", menuName = "SharedVariables/Float", order = 1)]
public class FloatSharedVariable : ScriptableObject
{
    public float Value = 0f;
}
