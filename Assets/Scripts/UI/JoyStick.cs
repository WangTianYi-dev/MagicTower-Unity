/*
 * file: JoyStick.cs
 * author: DeamonHook
 * feature: 虚拟摇杆
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class JoyStick : MonoBehaviour
    , IDragHandler, IPointerUpHandler, IPointerDownHandler
{

    private Image jsContainer, // 摇杆外面的框
                  joystick; // 摇杆本身

    private Vector2 inputVec = Vector2.zero;

    [Header("摇杆反应的延迟")]
    public float lagSetting = 0.05f;

    private float lag = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        jsContainer = GetComponent<Image>();

        // 摇杆为其唯一的子组件
        joystick = transform.GetChild(0).GetComponent<Image>();
    }

    bool CheckKbdInput()
    {
        Vector2Int dir = Vector2Int.zero;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            dir = Vector2Int.up;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            dir = Vector2Int.down;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            dir = Vector2Int.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            dir = Vector2Int.right;
        }
        if (dir != Vector2Int.zero)
        {
            GameManager.instance.MoveByDir(dir);
            
            return true;
        }
        return false;
    }



    public void OnDrag(PointerEventData ped)
    {
        if (lag > 0.0f)
        {
            lag -= Time.deltaTime;
            return;
        }

        Vector2 position = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            jsContainer.rectTransform,
            ped.position,
            ped.pressEventCamera,
            out position
            );


        float x = (position.x / jsContainer.rectTransform.rect.width);
        float y = (position.y / jsContainer.rectTransform.rect.height);
        inputVec = new Vector2(x, y);
        if (inputVec.magnitude > 1) inputVec = inputVec.normalized;

        
        joystick.rectTransform.anchoredPosition = new Vector2(
            inputVec.x * jsContainer.rectTransform.rect.width / 2,
            // 为什么这里要/2 ？因为摇杆的半径是0.5 ^_^
            inputVec.y * jsContainer.rectTransform.rect.height / 2
            );
    }

    public void OnPointerDown(PointerEventData ped)
    {
        jsContainer.color = new Color32(255, 255, 255, 255);
        joystick.color = new Color32(255, 255, 255, 255);
        lag = lagSetting;
        OnDrag(ped);
    }

    public void OnPointerUp(PointerEventData ped)
    {
        jsContainer.color = new Color32(255, 255, 255, 127);
        joystick.color = new Color32(255, 255, 255, 127);
        inputVec = Vector2.zero;
        joystick.rectTransform.anchoredPosition = Vector3.zero;
        lag = 0;
    }

    private bool CheckJSInput()
    {
        Vector2Int dir = Vector2Int.zero;
        if (inputVec.magnitude > 0.025)
        {
            if (Mathf.Abs(inputVec.x) > Mathf.Abs(inputVec.y))
            {
                if (inputVec.x > 0)
                {
                    dir = Vector2Int.right;
                }
                else
                {
                    dir = Vector2Int.left;
                }
            }
            else
            {
                if (inputVec.y > 0)
                {
                    dir = Vector2Int.up;
                }
                else
                {
                    dir = Vector2Int.down;
                }
            }
        }
        if (dir != Vector2Int.zero)
        {
            GameManager.instance.MoveByDir(dir);
            return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckKbdInput()) return;
        if (CheckJSInput()) return;
    }
}
