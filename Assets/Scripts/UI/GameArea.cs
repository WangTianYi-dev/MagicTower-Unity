/*
 * file: GameArea.cs
 * author: DeamonHook
 * feature: 触控消息传递
 */

using DG.Tweening.Plugins;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Image image;
    private RectTransform rectTran;



    private void Start()
    {
        image = GetComponent<Image>();
        rectTran = GetComponent<RectTransform>();
    }


    public void OnPointerDown(PointerEventData ped)
    {
        Vector2 position = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            image.rectTransform,
            ped.position,
            ped.pressEventCamera,
            out position
        );

        float x = position.x / image.rectTransform.rect.width + 0.5f;
        float y = position.y / image.rectTransform.rect.height + 0.5f;
        GameManager.instance.TouchDown(x, y);
    }

    public void OnPointerUp(PointerEventData ped)
    {
        Vector2 position = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            image.rectTransform,
            ped.position,
            ped.pressEventCamera,
            out position
        );

        float x = position.x / image.rectTransform.rect.width + 0.5f;
        float y = position.y / image.rectTransform.rect.height + 0.5f;
        GameManager.instance.TouchUp(x, y);
    }

    /// <summary>
    /// 生成UI物体，默认在格子中间
    /// </summary>
    /// <param name="go">prefab，默认锚点为中心</param>
    /// <param name="logicPos"></param>
    /// <param name="size"></param>
    public GameObject InstCanvasObject(GameObject GO, Vector2Int logicPos, Vector2Int logicalSize)
    {
        Vector2 cellSize = rectTran.sizeDelta / logicalSize;
        Vector2 pos = cellSize * (logicPos - (rectTran.sizeDelta / 2 / cellSize) + new Vector2(0.5f, 0.0f));
        print($"cellsize: {cellSize}, pos: {pos}");
        var newGO = Instantiate(GO, transform, true);
        newGO.transform.localScale = Vector2.one;
        newGO.GetComponent<RectTransform>().anchoredPosition = pos;
        return newGO;
    }
}
