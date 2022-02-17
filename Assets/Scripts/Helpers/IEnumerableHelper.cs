using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class IEnumerableHelper 
{

    public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
    {
        foreach (T item in enumeration)
        {
            action(item);
        }
    }

    public static List<T> ToList<T>(this T[,] array)
    {
        List<T> retList = new List<T>();
        for(int i = 0; i < array.GetLength(0); i++)
        {
            for(int j = 0; j < array.GetLength(1); j++)
            {
                retList.Add(array[i, j]);
            }
        }
        return retList;
    }

    public static T Random<T>(this IEnumerable<T> enumerable)
    {
        if (enumerable == null)
        {
            throw new ArgumentNullException(nameof(enumerable));
        }

        var list = enumerable as IList<T> ?? enumerable.ToList();
        return list.Count == 0 ? default : list[UnityEngine.Random.Range(0, list.Count)];
    }
}
