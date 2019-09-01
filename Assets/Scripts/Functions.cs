using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// Common functions
/// </summary>
public static class Functions
{
    /// <summary>
    /// Convert array of Vector3 to array of Vector2 by removing the z coordinate
    /// </summary>
    public static Vector2[] Vector3ToVector2(Vector3[] vectors)
    {
        return vectors.Select(v => new Vector2(v.x, v.y)).ToArray();
    }

    /// <summary>
    /// Find maximal element in array determined by Comparer comp
    /// </summary>
    public static T Maximum<T>(this T[] array, IComparer<T> comp) 
    {
        if (array == null || array.Length == 0)
            throw new ArgumentException(nameof(array) + " can't be empty");

        T maximum = array[0];
        for (int i = 1; i < array.Length; i++)
        {
            if (comp.Compare(maximum, array[i]) == -1)
            {
                maximum = array[i];
            }
        }
        return maximum;
    }
}
