using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Drag : MonoBehaviour
{
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            if(Physics.Raycast(ray,out RaycastHit hit))
            {
                float posX  = Mathf.Round(hit.point.x);
                float posZ = Mathf.Round(hit.point.z);
                transform.position = new Vector3(posX, 0.5f, posZ);                
            }
        }
    }
}
