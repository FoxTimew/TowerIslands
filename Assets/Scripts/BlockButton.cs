using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlockButton : MonoBehaviour
{
    public int index;
    [SerializeField] private TMP_Text text;
    void Update()
    {
        text.text = GameManager.instance.islandCreator.blocksCount[index].ToString();
        gameObject.SetActive(GameManager.instance.islandCreator.blocksCount[index] > 0);
    }
}
