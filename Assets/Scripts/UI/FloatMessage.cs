/*
 * file: FloatMessage.cs
 * author: DeamonHook
 * feature: 浮动信息（如伤害）的实现
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class FloatMessage : MonoBehaviour
{
    [Header("浮动时间")]
    public float floatDuration;

    [Header("浮动距离")]
    public float floatDistance;

    [Header("停留时间")]
    public float appendDuration;

    private Text text;


    public void Refresh(string message)
    {
        text.text = message;
    }


    private void DoFloat()
    {
        RectTransform rectTran = text.rectTransform;
        Color c = text.color;
        text.color = new Color(c.r, c.g, c.b, 0);
        Sequence seq = DOTween.Sequence();
        Tweener move1 = rectTran.DOMoveY(rectTran.position.y + floatDistance,
            floatDuration);
        Tweener alpha1 = text.DOColor(c, floatDuration);
        seq.Append(alpha1);
        seq.Join(move1);
    }


    private void Awake()
    {
        text = GetComponent<Text>();
    }


    // Start is called before the first frame update
    void Start()
    {
        DoFloat();
        Destroy(this.gameObject, floatDuration + appendDuration);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
