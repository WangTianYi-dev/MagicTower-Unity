/*
 * file: DirButton.cs
 * author: DeamonHook
 * feature: 方向键
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// 魔塔里方向键比摇杆好用的多
public class DirButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
	public static DirButton instance { get; private set; }

	public Vector2Int curDir { get; private set; } = Vector2Int.zero;
	private Image image;
	// Use this for initialization
	void Start()
	{
		instance = this;
		image = GetComponent<Image>();
	}

	private bool touchLock = false;

	public void OnPointerUp(PointerEventData ped)
	{
		touchLock = false;
		curDir = Vector2Int.zero;
	}

	public void OnPointerDown(PointerEventData ped)
	{
		touchLock = true;
		OnDrag(ped);
	}

	public void OnDrag(PointerEventData ped)
    {
		Vector2 position = Vector2.zero;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(
			image.rectTransform,
			ped.position,
			ped.pressEventCamera,
			out position
		);

		if (Mathf.Abs(position.x) > Mathf.Abs(position.y))
		{
			if (position.x > 0)
            {
				curDir = Vector2Int.right;
            }
			else
            {
				curDir = Vector2Int.left;
            }
		}
		else
		{
			if (position.y > 0)
            {
				curDir = Vector2Int.up;
            }
			else
            {
				curDir = Vector2Int.down;
            }
		}
    }

	void CheckKeyboardInput()
	{
		if (!touchLock)
		{
			if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
			{
				curDir = Vector2Int.up;
			}
			else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
			{
				curDir = Vector2Int.down;
			}
			else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
			{
				curDir = Vector2Int.right;
			}
			else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
			{
				curDir = Vector2Int.left;
			}
			else
			{
				curDir = Vector2Int.zero; // 还原初始状态
			}
		}
	}

	private void FixedUpdate()
	{
        CheckKeyboardInput();
	}
}

