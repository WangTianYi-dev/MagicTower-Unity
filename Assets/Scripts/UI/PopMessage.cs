/*
 * file: PopMessage.cs
 * author: DeamonHook
 * feature: 弹出信息
 */
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class PopMessage : MonoBehaviour
{
	private Text msg;

    [Header("显示时间")]
	public float msgTime = 1f;

	private float curTime = 0f; 


	// Use this for initialization
	void Start()
	{
		msg = transform.Find("Text").GetComponent<Text>();
		msg.text = "";
	}

	// Update is called once per frame
	void Update()
	{
		if (curTime > 0)
        {
			curTime -= Time.deltaTime;
			if (curTime <= 0)
			{
				CloseMsg();
			}
        }
	}

	public void ShowMsg(string message)
    {
		curTime = msgTime;
		msg.text = message;
		msg.DOFade(1, 0.3f);
    }


	private void CloseMsg()
	{
		msg.DOFade(0, 0.3f);
	}


}

