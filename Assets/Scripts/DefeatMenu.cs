using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DefeatMenu : MonoBehaviour
{
    [SerializeField] private Transform background;
    [SerializeField] private Transform defeat;

    private Tween tween;
    
    private void OnEnable()
    {
        background.transform.localScale = Vector3.zero;
        defeat.transform.localScale = Vector3.zero;
        UI_Manager.instance.CloseMenu(9);
        background.DOScale(Vector3.one*0.5f, 0.25f).SetEase(Ease.OutSine);
        defeat.DOScale(Vector3.one * 0.5f, 0.25f).SetEase(Ease.OutSine);
        tween = background.DORotate(new Vector3(0, 0, 10), 0.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);

    }

    public void Close()
    {
        background.DOScale(Vector3.zero, 0.25f).SetEase(Ease.OutSine);
        defeat.DOScale(Vector3.zero, 0.25f).SetEase(Ease.OutSine).OnComplete( ()=> gameObject.SetActive(false));
    }

    private void OnDisable()
    {
        GameManager.instance.ClearBuildings();
        tween.Kill();
        background.rotation= Quaternion.Euler(Vector3.zero);
    }
}
