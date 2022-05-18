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
            position = Input.mousePosition
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
    
    public static Vector2[] UpdatePoints(int range)
    {
        int i = 0;
        Vector2[] points = new Vector2[range*4];
        List<Vector2> finalPoints = new List<Vector2>();
        Vector2 zeroPoint = new Vector2(-1.84f * range, -1.335f * range);
        points[0] = zeroPoint;
        finalPoints.Add(zeroPoint + new Vector2(-3.56f * 0.5f, 0));
        finalPoints.Add(zeroPoint + new Vector2(0, -1.335f));
        for (i = 1; i < range+1; i++)
        {
            points[i] = points[i - 1] + new Vector2(3.56f, 0);
            finalPoints.Add(points[i] + new Vector2(-3.56f * 0.5f, 0));
            finalPoints.Add(points[i] + new Vector2(0, -1.335f));
        }
        finalPoints.Add(points[i-1] + new Vector2(3.56f * 0.5f, 0));
        finalPoints.Add(points[i-1] + new Vector2(0, 1.335f));
        for (i = range+1; i < range*2+1; i++)
        {
            
            points[i] = points[i - 1] + new Vector2(0.12f, 2.67f);
            finalPoints.Add(points[i] + new Vector2(3.56f * 0.5f, 0));
            finalPoints.Add(points[i] + new Vector2(0, 1.335f));
            
        }
        finalPoints.Add(points[i-1] + new Vector2(0, 1.335f));
        finalPoints.Add(points[i-1] + new Vector2(-3.56f * 0.5f, 0));
        for (i = range*2+1; i < range*3+1; i++)
        {
            points[i] = points[i - 1] + new Vector2(-3.56f, 0);
            finalPoints.Add(points[i] + new Vector2(0, 1.335f));
            finalPoints.Add(points[i] + new Vector2(-3.56f * 0.5f, 0));
            
        }
        finalPoints.Add(points[i-1] + new Vector2(-3.56f * 0.5f, 0));
        finalPoints.Add(points[i-1] + new Vector2(0, -1.335f));
        for (i = range*3+1; i < range*4; i++)
        {
            points[i] = points[i - 1] + new Vector2(-0.12f, -2.67f);
            finalPoints.Add(points[i] + new Vector2(-3.56f * 0.5f, 0));
            finalPoints.Add(points[i] + new Vector2(0, -1.335f));
        }
        return finalPoints.ToArray();
    }
}
