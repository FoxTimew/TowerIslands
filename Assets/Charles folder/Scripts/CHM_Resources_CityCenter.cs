using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CHM_Resources_CityCenter : MonoBehaviour
{
    public float gold;

    public TMP_Text goldText;
    void Start()
    {
        ReloadFunds();
    }

    public void ReloadFunds()
    {
        goldText.text = "Gold : " + gold.ToString();
    }
}
