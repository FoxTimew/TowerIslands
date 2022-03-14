using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private List<Block> adjacentBlocks;
    public int energy = 2;
 
        
    public bool selected;

    [SerializeField] private GameObject building;
    
    [SerializeField] private Material selectedMat;
    [SerializeField] private Material initMat;

    [SerializeField] private MeshRenderer meshRenderer;
    
    Vector3[] InitAdjacents()
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
        foreach (Vector3 adj in InitAdjacents())
        {
            if (!GameManager.instance.blocks.ContainsKey(adj)) continue;
            adjacentBlocks.Add(GameManager.instance.blocks[adj]);
        }
    }


    public void Select()
    {
        selected = true;
        meshRenderer.material = selectedMat;
    }

    public void Deselect()
    {
        selected = false;
        meshRenderer.material = initMat;
    }

    public int GetMaxEnergy()
    {
        int result = energy;
        foreach (var block in adjacentBlocks)
        {
            result += block.energy;
        }
        return result;
    }
    

    

}
