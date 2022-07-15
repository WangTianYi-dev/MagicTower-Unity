/*
 * file: BaseManager.cs
 * author: DeamonHook
 * date: 7/7/2022
 * feature: 管理器的抽象基类
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour
{
    protected ResResolver resolver;

    public virtual void Init()
    {
        resolver = ResResolver.instance;
    }
}
