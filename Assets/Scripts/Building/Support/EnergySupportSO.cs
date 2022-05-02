using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnergySupportSO", menuName = "ScriptableObjects/Support/EnergySupportSO", order = 1)]
public class EnergySupportSO : SupportSO
{
    public int energyValue;

    private Block block;
    public override void AddEffect(GameObject go)
    {
        block.energy += energyValue;
    }

    public override void RemoveEffect(GameObject go)
    {
        block.energy -= energyValue;
        if(block.energy < 0) block.DestroyBuilding();
    }
    
    public override void Enter(Collider2D other, ref Dictionary<GameObject,int> dic)
    {
        if (other.transform.CompareTag("Block"))
        {
            block = other.gameObject.GetComponent<Block>();
            AddEffect(other.gameObject);
        }
    }
    
}
