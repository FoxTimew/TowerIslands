using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Utils
{
    public static Vector2 Round(Vector2 vec)
    {
        vec.x = (float)Math.Round(vec.x, 2);
        vec.y = (float)Math.Round(vec.y, 2);
        return vec;
    }
    
    public static bool IsPointerOverUI()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }

        var pe = new PointerEventData(EventSystem.current)
        {
            position = Input.GetTouch(0).position
        };
        var hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pe, hits);
        return hits.Count > 0;
    }

    public static List<Index> GetAdjacentsIndex(Index index)
    {
        List<Index> result = new List<Index>();
        Index[] temp =  new[]
        {
            new Index(index.x + 1, index.y),
            new Index(index.x - 1, index.y),
            new Index(index.x, index.y + 1),
            new Index(index.x, index.y - 1),
        };
        
        foreach (var i in temp)
        {
            if (i.x < 0 || i.x >= GameManager.instance.grid.size || i.y < 0 || i.y >=  GameManager.instance.grid.size) continue;
            if(GameManager.instance.grid.GridElements[i.x,i.y].walkable)
                result.Add(i);
        }

        return result;
    }
}
