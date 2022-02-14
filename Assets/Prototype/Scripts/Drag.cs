using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    private Vector3 offset;

    private void Update()
    {
        if (Input.touchCount < 1) return;
        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
            offset = transform.position - Building.GetTouchWorldPosition();
        if (touch.phase == TouchPhase.Moved)
        {
            Vector3 pos = Building.GetTouchWorldPosition() + offset;
            transform.position = Building.current.SnapToGrid(pos);
        }
        
    }
}
