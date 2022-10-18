/*
 * file: EffectLayer.cs
 * author: DeamonHook
 * feature: 特效层
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectLayer : MonoBehaviour
{
    public static EffectLayer instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    private void DeleteChilds()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// 创建SpriteEffect
    /// </summary>
    /// <param name="pos">位置</param>
    /// <param name="prefab">effect对象</param>
    public void CreateSpriteEffect(Vector2Int pos, GameObject prefab)
    {
        var obj = Util.Inst(prefab, transform, pos );
        obj.GetComponent<SpriteRenderer>().sortingLayerName = "Effect";
    }

}
