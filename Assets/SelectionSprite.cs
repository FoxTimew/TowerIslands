using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionSprite : MonoBehaviour
{
    [SerializeField] private GameObject occupied;
    [SerializeField] private GameObject empty;
    
    void Update()
    {
        if (GameManager.instance.selectedBlock is not null)
        {
            transform.position = GameManager.instance.selectedBlock.transform.position;
            if (GameManager.instance.selectedBlock.building is null)
            {
                empty.SetActive(true);
                occupied.SetActive(false);
            }
            else
            {
                empty.SetActive(false);
                occupied.SetActive(true);
            }
        }
        else
        {
            empty.SetActive(false);
            occupied.SetActive(false);
        }
    }
}
