using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPowerUp : RandomCollectable
{
    protected override GameObject GetCollectable()
    {
        return RandomCollectableSystem.Instance.GetRandomPowerUp();
    }
}
