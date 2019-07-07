using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class Functions
{
    public static Vector2[] Vector3ToVector2(Vector3[] vectors)
    {
        return vectors.Select(v => new Vector2(v.x, v.y)).ToArray();
    }
}
