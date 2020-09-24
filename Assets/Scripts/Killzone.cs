using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Killzone : MonoBehaviour
{
    public Vector3 center = Vector3.zero;
    public Vector3 Extents = Vector3.one;

    private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Killzone");
        GetComponent<Rigidbody>().isKinematic = true;
        Extents = GetComponent<BoxCollider>().size;
        center = transform.position;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision == null || !collision.gameObject.activeInHierarchy) return;
        collision.transform.parent.gameObject.SetActive(false); // Get endposition, set parent inactive
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(center, Extents);
    }
#endif
}
