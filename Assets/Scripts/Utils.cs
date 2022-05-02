using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static Vector2 Round(Vector2 vec)
    {
        vec.x = (float)Math.Round(vec.x, 2);
        vec.y = (float)Math.Round(vec.y, 2);
        return vec;
    }
}
