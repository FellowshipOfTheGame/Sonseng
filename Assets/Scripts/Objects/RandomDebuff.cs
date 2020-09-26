using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDebuff : RandomCollectable
{
    protected override GameObject GetCollectable()
    {
        return RandomCollectableSystem.Instance.GetRandomDebuff();
    }
}
