using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class WaveCleared : MonoBehaviour
{
    [SerializeField] private Image image;
    private void OnDisable()
    {
        image.color = Color.white;
    }
}
