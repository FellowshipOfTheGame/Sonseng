using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollectable : RandomCollectable
{
    protected override GameObject GetCollectable()
    {
        return RandomCollectableSystem.Instance.GetCoin();
    }
}
