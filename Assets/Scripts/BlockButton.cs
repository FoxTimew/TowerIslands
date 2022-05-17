using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlockButton : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    void Update()
    {
        text.text = GameManager.instance.islandCreator.blocksCount[name].ToString();
        gameObject.SetActive(GameManager.instance.islandCreator.blocksCount[name] > 0);
    }
}
