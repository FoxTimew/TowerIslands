using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CHM_SpellsManager : MonoBehaviour
{
    public CHM_PowerUpRay powerUpRay;

    public bool canCastRay;
    public TMP_Text cooldownText;

    public float reloadRemaining;
    private void Start()
    {
        canCastRay = true;
    }
    private void Update()
    {
        if (powerUpRay.spellCalled == true && canCastRay == true)
        {
            //if(Input.touchCount > 0)
            //{
            //    touch = Input.GetTouch(0);
            //}
            if (Input.GetMouseButtonDown(0) == true)
            {
                powerUpRay.circleCollider.enabled = true;
                powerUpRay.spellCalled = false;
                powerUpRay.CastRay(Input.mousePosition);
                canCastRay = false;
            }
        }

        if (powerUpRay.reloading == true)   
        {
            cooldownText.text = reloadRemaining.ToString();
        }
    }

    public void SelectRayButton()
    {
        if(canCastRay == true)
        {
            if(powerUpRay.spellCalled == false)
            {
                powerUpRay.spellCalled = true;
            }

            else if (powerUpRay.spellCalled == true)
            {
                powerUpRay.spellCalled = false;
            }
        }
    }
}
