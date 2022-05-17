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
        PlayAnimation();

    }

    public void CloseContextMenu()
    {
        transform.DOScale(Vector3.zero, animationTime).OnComplete(
            ()=>gameObject.SetActive(false));
    }



    public void PlayAnimation()
    {
        transform.DOScale(Vector3.one*2, animationTime);
    }
}
