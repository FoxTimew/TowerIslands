using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CircleMenuAnimation : MonoBehaviour
{
    [SerializeField] private static float animationTime = 0.25f;
    private void Start()
    {
        transform.DOScale(Vector3.one, animationTime);
    }

    public void CloseContextMenu()
    {
        transform.DOScale(Vector3.zero, animationTime);
        gameObject.SetActive(false);
    }
    
    
}
