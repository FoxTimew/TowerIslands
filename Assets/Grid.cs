using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
public class Grid : MonoBehaviour
{
    [SerializeField] private List<GameObject> gridLines;

    public bool[,] walkable;
    public Vector2[,] position;
    void Start()
    {
        int temp = 0;
        int temp1 = 0;
        position = new Vector2[10, 10];
        walkable = new bool[10, 10];
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                position[i,j] = (Vector2)Utils.Round(gridLines[i].transform.GetChild(j).transform.position);
                if (GameManager.instance.blocks.ContainsKey(Utils.Round(gridLines[i].transform.GetChild(j).transform
                    .position)))
                {
                    walkable[i, j] = true;
                    temp1++;
                    gridLines[i].transform.GetChild(j).gameObject.SetActive(false);
                }
                else
                {
                    walkable[i, j] = false;
                    gridLines[i].transform.GetChild(j).gameObject.SetActive(true);
                }
            }
        }
        Debug.Log(temp1);
    }
}
