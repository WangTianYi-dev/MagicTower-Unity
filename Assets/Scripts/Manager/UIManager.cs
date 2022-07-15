/*
 * file: UIManager.cs
 * author: DeamonHook
 * date: 7/9/2022
 * feature: UIπ‹¿Ì∆˜
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : BaseManager
{
    public static UIManager instance { get; private set; }

    public override void Init()
    {
        base.Init();
        instance = this;
        //m = transform.Find("Canvas/MessageWindow").GetComponent<MessageWindow>();

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
