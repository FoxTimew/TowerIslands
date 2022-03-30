using System.Collections.Generic;
using UnityEngine;
public class SupportTower : MonoBehaviour
{

    
    [SerializeField] private AXD_BuildingSO stats;
    [SerializeField] private int range;
    [SerializeField] private List<Block> inRangeBlock = new List<Block>();
    

    void Start()
    {
        GetBlockInRange();
    }


    void GetBlockInRange()
    {
        float xRange = range * 0.5f + transform.position.x;
        float yRange;
        for (float x = -xRange; x <= xRange; x += 0.5f)
        {
            yRange = range * 0.25f + transform.position.y;
            if (x % 1 != 0)
                yRange -= 0.25f;

            for (float y = -yRange; y <= yRange; y += 0.5f)
            {
                if (GameManager.instance.blocks.ContainsKey(new Vector2(x, y)))
                    inRangeBlock.Add(GameManager.instance.blocks[new Vector2(x, y)]);
            }
        }
    }
}
