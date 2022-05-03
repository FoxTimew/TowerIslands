using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
public class Grid : MonoBehaviour
{
    [SerializeField] private List<GameObject> gridLines;

    public int[,] walkable;
    public Vector2[,] position;
    void Start()
    {
        position = new Vector2[10, 10];
        walkable = new int[10, 10];
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                position[i,j] = (Vector2)Utils.Round(gridLines[i].transform.GetChild(j).transform.position);
                if (GameManager.instance.blocks.ContainsKey(gridLines[i].transform.GetChild(j).transform
                    .position))
                {
                    walkable[i, j] = 1;
                    gridLines[i].transform.GetChild(j).gameObject.SetActive(false);
                }
                else
                {
                    walkable[i, j] = 0;
                    gridLines[i].transform.GetChild(j).gameObject.SetActive(true);
                }
            }
        }
    }
}
