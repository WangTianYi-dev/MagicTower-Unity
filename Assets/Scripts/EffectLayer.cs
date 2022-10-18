/*
 * file: EffectLayer.cs
 * author: DeamonHook
 * feature: ��Ч��
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
    /// ����SpriteEffect
    /// </summary>
    /// <param name="pos">λ��</param>
    /// <param name="prefab">effect����</param>
    public void CreateSpriteEffect(Vector2Int pos, GameObject prefab)
    {
        var obj = Util.Inst(prefab, transform, pos );
        obj.GetComponent<SpriteRenderer>().sortingLayerName = "Effect";
    }

}
