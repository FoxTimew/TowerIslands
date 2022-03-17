using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    public Block initPos;
    public Block destination;
    void Start()
    {
        transform.position = initPos.transform.position;
        Pathfinding pf = new Pathfinding();
        List<Vector2> sheeeesh = new List<Vector2>();
        foreach (var block in GameManager.instance.blocks.Values)
        {
            sheeeesh.Add(block.transform.position);
        }
        List<Pathfinding.Node> sheeesh = Pathfinding.instance.FindPath(initPos.transform.position, destination.transform.position, sheeeesh);

        foreach (var node in sheeesh)
        {
            Debug.Log(node.pos);
            GameManager.instance.blocks[node.pos].GetComponent<SpriteRenderer>().color = Color.green;
        }

        //StartCoroutine(MoveEnemy(sheeesh));
    }
    
    IEnumerator MoveEnemy(List<Pathfinding.Node> sheeeesh)
    {
        for (int i = 0; i < sheeeesh.Count; i++)
        {
            transform.DOMove(sheeeesh[i].pos, 0.5f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(0.5f);
        }
        transform.DOMove(destination.transform.position, 0.5f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.5f);
    }
}
