using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class WaveTransitionAnimation : MonoBehaviour
{
    public Image waveClearedImage;
    public Image vignetteImage;
    [Range(0, 255)] public int vignetteMaxAlpha = 50;
    public Image buildYourDefensesImage;
    public float appearingTime = 0.5f;
    public float disappearingTime = 0.5f;
    public float imageShowTime = 1f;

    private void OnEnable()
    {
        Debug.Log("");
        StartCoroutine(WaveClearedAnimationCoroutine());
    }

    private void VignetteFadeIn()
    {
        vignetteImage.DOFade((float)vignetteMaxAlpha/255, appearingTime);
    }

    public void VignetteFadeOut()
    {
        vignetteImage.DOFade(0, disappearingTime);
    }

    private void WaveClearShow()
    {
        waveClearedImage.transform.DOScale(Vector3.one, appearingTime);
    }

    private void WaveClearTransitionBuildYourDefenses()
    {
        VignetteFadeOut();
        waveClearedImage.DOFade(0, disappearingTime).OnComplete(() =>
        {
            buildYourDefensesImage.DOFade(1, appearingTime);
        });
    }
    private void BuildDefenseDisappear()
    {
        buildYourDefensesImage.DOFade(0, disappearingTime).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
        
    }

    private IEnumerator WaveClearedAnimationCoroutine()
    {
        Debug.Log("Coroutine launched");
        WaveClearShow();
        VignetteFadeIn();
        yield return new WaitForSeconds(imageShowTime);
        WaveClearTransitionBuildYourDefenses();
        yield return new WaitForSeconds(imageShowTime+appearingTime);
        BuildDefenseDisappear();
    }
}
