/*
 * file: BaseWindow.cs
 * author: DeamonHook
 * feature: 所有窗口的抽象基类
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWindow : MonoBehaviour
{
    public virtual void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public virtual void InitWnd()
    {
        SetActive(false);
    }
}
