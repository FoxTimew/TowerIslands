using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragPointer : MonoBehaviour
{
    
    public bool isSnapped = false;
    public Index index = new Index(0,0);
    public Vector3 snapPosition;
    
    void Update()
    {
        transform.localPosition = Vector3.zero;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<GridIndex>())
        {
            index = other.GetComponent<GridIndex>().index;
            snapPosition = GameManager.instance.grid.GridElements[index.x, index.y].position;
        }
        if (other.transform.CompareTag("Grid"))
        {
            isSnapped = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.CompareTag("Grid"))
        {
            isSnapped = false;
        }
    }
    
}
