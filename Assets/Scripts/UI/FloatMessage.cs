/*
 * file: FloatMessage.cs
 * author: DeamonHook
 * feature: ������Ϣ�����˺�����ʵ��
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class FloatMessage : MonoBehaviour
{
    [Header("����ʱ��")]
    public float floatDuration;

    [Header("��������")]
    public float floatDistance;

    [Header("ͣ��ʱ��")]
    public float appendDuration;


    public void Refresh(string message)
    {
        print($"Refresh: {message}");
        GetComponent<Text>().text = message;
    }


    private void DoFloat()
    {
        Color c = GetComponent<Text>().color;
        GetComponent<Text>().color = new Color(c.r, c.g, c.b, 0);
        Sequence seq = DOTween.Sequence();
        Tweener move1 = GetComponent<RectTransform>().DOMoveY(GetComponent<RectTransform>().position.y + floatDistance,
            floatDuration);
        Tweener alpha1 = GetComponent<Text>().DOColor(c, floatDuration);
        seq.Append(alpha1);
        seq.Join(move1);
    }


    // Start is called before the first frame update
    void Start()
    {
        DoFloat();
        Destroy(this.gameObject, floatDuration + appendDuration);
    }

}
