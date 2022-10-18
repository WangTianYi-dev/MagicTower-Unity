/*
 * file: Util.cs
 * author: DeamonHook
 * feature: 工具函数
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour
{
    /// <summary>
    /// 实例化prefab, 使得其为father的子物体
    /// </summary>
    /// <param name="prefab">被实例化的prefab</param>
    /// <param name="father">其父物体</param>
    /// <param name="offset">子物体位置相对于父物体的偏置量</param>
    public static GameObject Inst(GameObject prefab, Transform fatherTrans, Vector2Int offset)
    {
        GameObject obj = Instantiate(prefab, fatherTrans, true);
        Vector3 pos = new Vector3(fatherTrans.position.x + offset.x, fatherTrans.position.y + offset.y, fatherTrans.position.z);
        obj.transform.position = pos;
        return obj;
    }

    public static Vector2Int GetPosition(Transform transform)
    {
        var relativePos = transform.position - transform.parent.transform.position;
        return new Vector2Int((int)Mathf.Round(relativePos.x), (int)Mathf.Round(relativePos.y));
    }

    public static Vector3 ApplyOffset(GameObject obj, Vector2Int pos)
    {
        return new Vector3(obj.transform.parent.position.x + pos.x, obj.transform.parent.position.y + pos.y);
    }

    /// <summary>
    /// 将地图数组转化为对应的字符串
    /// </summary>
    /// <param name="map">地图数组</param>
    /// <returns>字符串</returns>
    public static string MapToString(System.Object[,] map)
    {
        string s = "";
        for (int y = map.GetLength(1) - 1; y >= 0; y--)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                s += map[x, y].ToString() + ' ';
            }
            s += '\n';
        }
        return s;
    }

    /// <summary>
    /// 检查i是否在[left, right]的闭区间内
    /// </summary>
    /// <param name="i">欲检查的数</param>
    /// <param name="left">下界</param>
    /// <param name="right">上界</param>
    /// <returns>是否在区间内</returns>
    public static bool Inside(int i, int left, int right)
    {
        return left <= i && i <= right;
    }

    /// <summary>
    /// 检查点cordin是否在以leftDown为左下角且以rightUp为右上角的矩形闭区间内
    /// </summary>
    /// <param name="cordin"></param>
    /// <param name="leftDown"></param>
    /// <param name="rightUp"></param>
    /// <returns>是否在区间内</returns>
    public static bool Inside(Vector2Int cordin, Vector2Int leftDown, Vector2Int rightUp)
    {
        return Inside(cordin.x, leftDown.x, rightUp.x) && Inside(cordin.y, leftDown.y, rightUp.y);
    }


    public static void ClearChildren(Transform tr)
    {
        for (int i = 0; i < tr.childCount; i++)
        {
            Destroy(tr.GetChild(i).gameObject);
        }
    }


    public static void RemoveEntity(Entity e)
    {
        MapManager.instance.RemoveEntity(e);
    }
}
