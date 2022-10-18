/*
 * file: Router.cs
 * author: DeamonHook
 * feature: 寻路
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Router
{


    private static Vector2Int[] dirs =
    {
        Vector2Int.left, Vector2Int.up, Vector2Int.right, Vector2Int.down
    };



    /// <summary>
    /// 获取从start到end的路径
    /// </summary>
    /// <param name="passableGrid">格子的可通过性矩阵</param>
    /// <param name="start">起点</param>
    /// <param name="end">终点</param>
    /// <returns>起点到终点的最短路径，若不存在路径则返回空链表（包括起点和终点）</returns>
    public static LinkedList<Vector2Int> Route(bool[,] passableGrid, Vector2Int start, Vector2Int end)
    {
        int maxX = passableGrid.GetLength(0), maxY = passableGrid.GetLength(1);
        Dictionary<Vector2Int, Vector2Int?> preDict = new Dictionary<Vector2Int, Vector2Int?>();
        var q = new Queue<Vector2Int>();
        q.Enqueue(start);
        preDict[start] = null;
        while (q.Count > 0)
        {
            Vector2Int c = q.Dequeue();

            foreach (var dv in dirs)
            {
                var nv = c + dv;

                if (nv == end)
                {
                    var res = new LinkedList<Vector2Int>();
                    res.AddFirst(nv);
                    Vector2Int? t = c;
                    while (t != null)
                    {
                        res.AddFirst((Vector2Int)t);
                        t = preDict[(Vector2Int)t];
                    }
                    return res;
                }
                if (!preDict.ContainsKey(nv))
                {
                    preDict[nv] = null;
                    if (Util.Inside(nv, new Vector2Int(0, 0), new Vector2Int(maxX - 1, maxY - 1)))
                    {
                        if (passableGrid[nv.x, nv.y])
                        {
                            preDict[nv] = c;
                            q.Enqueue(nv);
                        }
                    }
                }
            }
        }
        return new LinkedList<Vector2Int>(); // return empty
    }
}
