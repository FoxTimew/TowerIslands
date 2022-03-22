using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    public Block initPos;
    public Block destination;
    void Start()
    {
        destination = GameManager.instance.blocks[new Vector2(0, -0.25f)];
        StartCoroutine(MoveEnemy());
    }
    
    IEnumerator MoveEnemy()
    {
        var des = GameManager.instance.blocks.Keys.First();
        foreach (var pos in GameManager.instance.blocks.Keys)
        {
            if ((transform.position - (Vector3)des).magnitude > (transform.position - (Vector3)pos).magnitude)
            {
                des = pos;
            }
        }
        transform.DOMove(des, 2f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(2f);
        
        initPos = GameManager.instance.blocks[transform.position];
        Pathfinding pf = new Pathfinding();
        List<Vector2> map = new List<Vector2>();
        foreach (var block in GameManager.instance.blocks.Values)
        {
            map.Add(block.transform.position);
        }
        List<Pathfinding.Node> path = Pathfinding.instance.FindPath(initPos.transform.position, destination.transform.position, map);

        foreach (var node in path)
        {
            GameManager.instance.blocks[node.pos].GetComponent<SpriteRenderer>().color = Color.green;
        }
        
        for (int i = 0; i < path.Count; i++)
        {
            transform.DOMove(path[i].pos, 0.5f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(0.5f);
        }
        transform.DOMove(destination.transform.position, 0.5f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.5f);
    }
}
