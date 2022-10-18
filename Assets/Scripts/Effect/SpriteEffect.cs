/*
 * file: SpriteEffect.cs
 * author: DeamonHook
 * feature: 由精灵动画组成的效果组件
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 为简单的效果创建Animator状态机太麻烦了，所以我决定采用这种简单的方式
public class SpriteEffect : MonoBehaviour
{
    [Header("销毁延迟")]
    public float delay = 0.0f;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        GetComponent<SpriteRenderer>().sortingLayerName = "Effect";
    }

    // Start is called before the first frame update
    void Start()
    {
        if (animator != null)
        {
            Destroy(
                gameObject,
                animator.GetCurrentAnimatorStateInfo(0).length + delay
            );
        }
        else
        {
            Destroy(
                gameObject,
                0.2f + delay
            );
        }
    }

    
}
