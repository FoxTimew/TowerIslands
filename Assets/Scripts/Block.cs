using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private List<Block> adjacentBlocks;
    public int energy = 2;


    Vector3[] InitAdajacents()
    {
        float posX = transform.position.x;
        float posZ = transform.position.z;
        return new[]
        {
            new Vector3(posX+1,0,posZ),
            new Vector3(posX-1,0,posZ),
            new Vector3(posX,0,posZ+1),
            new Vector3(posX,0,posZ-1),
        };
    }
    
    public void FindAdjacents()
    {
        foreach (Vector3 adj in InitAdajacents())
        {
            if (!GameManager.instance.blocks.ContainsKey(adj)) continue;
            adjacentBlocks.Add(GameManager.instance.blocks[adj]);
        }
    }

    

}
