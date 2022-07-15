/*
 * file: BaseWindow.cs
 * author: DeamonHook
 * date: 10/7/2022
 * feature: 所有窗口的抽象基类
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWindow : MonoBehaviour
{
    public virtual void SetState(bool active)
    {
        gameObject.SetActive(active);
    }

    public virtual void Init()
    {
        SetState(false);
    }
}
