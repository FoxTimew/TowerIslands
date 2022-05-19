using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridIndex : MonoBehaviour
{
    public Index index;
    [SerializeField] private PolygonCollider2D collider;
    public void Disable()
    {
        collider.enabled = false;
    }
}
