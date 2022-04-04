using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AXD_City : MonoBehaviour
{
    public static AXD_City instance;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    
}
