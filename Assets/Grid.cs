using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
public class Grid : MonoBehaviour
{
    [SerializeField] private List<GameObject> gridLines;


    public int size = 10;
    public GameObject prefab;
    public bool[,] walkable;
    public Vector2[,] position;

    public Vector2 zeroPos;
    void Start()
    {
        position = new Vector2[size, size];
        /*walkable = new bool[10, 10];
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                position[i,j] = (Vector2)Utils.Round(gridLines[i].transform.GetChild(j).transform.position);
                if (GameManager.instance.blocks.ContainsKey(Utils.Round(gridLines[i].transform.GetChild(j).transform
                    .position)))
                {
                    walkable[i, j] = true;
                    gridLines[i].transform.GetChild(j).gameObject.SetActive(false);
                }
                else
                {
                    walkable[i, j] = false;
                    gridLines[i].transform.GetChild(j).gameObject.SetActive(true);
                }
            }
        }*/

        Debug.Log(Mathf.Round(size*0.5f));
        zeroPos = new Vector2( -0.06f + (-0.12f * (Mathf.Round(size*0.5f)-1)),  -1.335f + (-2.67f * (Mathf.Round(size*0.5f)-1)));


        for (int i = 0; i < size; i++)
        {
            Vector2 tempZero = zeroPos + new Vector2(-1.72f *i, 1.335f*i);
            for (int j = 0; j < size; j++)
            {

                Vector2 temp = tempZero + new Vector2(1.84f * j, 1.335f * j);
                Instantiate(prefab, temp, Quaternion.identity);
                position[i, j] = temp;
            }
           
        }
    }
}
