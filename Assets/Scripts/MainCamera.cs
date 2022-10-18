/*
 * file: MainCamera.cs
 * author: DeamonHook
 * feature: 主相机
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    
    
    private Vector3 originalPos;
    private const float dTime = 0.2f;
    private float curTime = 0.2f;

    private const float range = 0.03f;

    private bool shaking;
    
    public void Reset()
    {
        shaking = false;
        transform.position = originalPos;
    }
    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (shaking)
        {
            if (curTime > 0)
            {
                curTime -= Time.deltaTime;
                if (curTime <= 0)
                {
                    curTime = dTime;
                    transform.position = originalPos + new Vector3(Random.Range(-range, range), Random.Range(-range, range), 0);
                }
            }
        }
    }

    public void Shake()
    {
        shaking = true;
    }
}
