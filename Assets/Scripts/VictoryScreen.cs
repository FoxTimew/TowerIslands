using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using DG.Tweening;
using UnityEngine;

public class VictoryScreen : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;
    public Transform victory;
    public Transform sun;
    public GameObject transition;

    private Tween tween;

    private void OnEnable()
    {
        transition.SetActive(false);
        particleSystem.Play();
        UI_Manager.instance.CloseMenu(9);
        sun.DOScale(Vector3.one*0.5f, 0.25f).SetEase(Ease.OutSine);
        victory.DOScale(Vector3.one * 0.5f, 0.25f).SetEase(Ease.OutSine)
            .OnComplete(StartReset);
        tween = sun.DORotate(new Vector3(0, 0, 10), 0.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);

    }

    private WaitForSeconds screenTime = new WaitForSeconds(4.5f);

    void StartReset()
    {
        StartCoroutine(ResetScale());
    }
    IEnumerator ResetScale()
    {
        yield return screenTime;
        victory.DOScale(Vector3.zero, 0.25f).SetEase(Ease.OutSine);
        sun.DOScale(Vector3.zero, 0.25f).SetEase(Ease.OutSine).OnComplete(OpenClose);
        
    }

    void OpenClose()
    {
        UI_Manager.instance.CloseMenu(12);
        UI_Manager.instance.OpenMenu(4);
        particleSystem.Stop();
    }

    private void OnDisable()
    {
        tween.Kill();
        sun.rotation= Quaternion.Euler(Vector3.zero);
    }
    
}


