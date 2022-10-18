/*
 * file: SpriteEffect.cs
 * author: DeamonHook
 * feature: �ɾ��鶯����ɵ�Ч�����
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ϊ�򵥵�Ч������Animator״̬��̫�鷳�ˣ������Ҿ����������ּ򵥵ķ�ʽ
public class SpriteEffect : MonoBehaviour
{
    [Header("�����ӳ�")]
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
