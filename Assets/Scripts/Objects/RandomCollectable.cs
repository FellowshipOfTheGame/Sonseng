using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class RandomCollectable : MonoBehaviour
{

    private GameObject _activeCollectable = null;

    private void Awake()
    {
#if UNITY_EDITOR
        Assert.IsNotNull(RandomCollectableSystem.Instance, $"RandomCollectableSystem instance is null for {name}");
#endif
    }

    private void OnEnable()
    {
        _activeCollectable = GetCollectable();
        _activeCollectable.transform.SetPositionAndRotation(transform.position, transform.rotation);
        _activeCollectable.SetActive(true);
        _activeCollectable.transform.SetParent(transform);
        PowerUps.instance.OnPowerPicked += PowerUps_OnPowerPicked;
    }

    private void PowerUps_OnPowerPicked()
    {
        if(_activeCollectable != null && _activeCollectable.layer != LayerMask.NameToLayer("Coin"))
        {
            Destroy(_activeCollectable);
            _activeCollectable = RandomCollectableSystem.Instance.GetCoin();
            _activeCollectable.transform.SetPositionAndRotation(transform.position, transform.rotation);
            _activeCollectable.SetActive(true);
            _activeCollectable.transform.SetParent(transform);
        }
    }

    private void OnDisable()
    {
        if (_activeCollectable != null)
        {
            Destroy(_activeCollectable);
        }
        PowerUps.instance.OnPowerPicked -= PowerUps_OnPowerPicked;
    }

    protected virtual GameObject GetCollectable()
    {
        return RandomCollectableSystem.Instance.GetRandomCollectable();
    }
}