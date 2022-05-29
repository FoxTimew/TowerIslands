using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Image timer;
    private void OnEnable()
    {
        timer.DOFillAmount(0,10).SetEase(Ease.Linear);
    }

    private void OnDisable()
    {
        timer.fillAmount = 1;
    }
}
