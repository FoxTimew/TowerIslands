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
    
    public void WaveClearedTransition()
    {
        StartCoroutine(WaveClearedAnimationCoroutine());
    }

    public void BuildYourDefenseTransition()
    {
        StartCoroutine(BuildYourDefensesCoroutine());
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
        waveClearedImage.gameObject.SetActive(true);
        waveClearedImage.transform.DOScale(Vector3.one, appearingTime);
    }

    private void WaveClearTransitionBuildYourDefenses()
    {
        Debug.Log("Wave Cleared Transition");
        VignetteFadeOut();
        waveClearedImage.DOFade(0, disappearingTime).OnComplete(() =>
        {
            waveClearedImage.transform.localScale = Vector3.zero;
            Debug.Log($"waveCleared scale : {waveClearedImage.transform.localScale.x},{waveClearedImage.transform.localScale.y},{waveClearedImage.transform.localScale.z}");
            waveClearedImage.color = Color.white;
            buildYourDefensesImage.DOFade(1, appearingTime);
            waveClearedImage.gameObject.SetActive(false);
        });
    }
    
    private void BuildDefenseShow()
    {
        buildYourDefensesImage.DOFade(1, disappearingTime);
        
    }
    private void BuildDefenseDisappear()
    {
        buildYourDefensesImage.DOFade(0, disappearingTime).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
        
    }

    private IEnumerator BuildYourDefensesCoroutine()
    {
        Debug.Log("Build Your defense Coroutine launched");
        BuildDefenseShow();
        yield return new WaitForSeconds(imageShowTime + appearingTime);
        BuildDefenseDisappear();
    } 
    public IEnumerator WaveClearedAnimationCoroutine()
    {
        Debug.Log("Wave Cleared Coroutine launched");
        WaveClearShow();
        VignetteFadeIn();
        yield return new WaitForSeconds(imageShowTime);
        WaveClearTransitionBuildYourDefenses();
        yield return new WaitForSeconds(imageShowTime+appearingTime);
        BuildDefenseDisappear();
    }
}
