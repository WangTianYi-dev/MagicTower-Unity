/*
 * file: GroundLayer.cs
 * author: DeamonHook
 * feature: 地面层
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundLayer : MonoBehaviour
{
    public static GroundLayer instance { get; private set; }

    private LineRenderer lineRenderer;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void DeleteChilds()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public List<Entity> GetEntities()
    {
        var l = new List<Entity>();
        for (int i = 0; i < transform.childCount; i++)
        {
            var o = transform.GetChild(i);
            if (o.GetComponent<Entity>() != null)
            {
                l.Add(o.GetComponent<Entity>());
            }
        }
        return l;
    }

    public Dictionary<Vector2Int, Entity> entityDict = new Dictionary<Vector2Int, Entity>();


    public GameObject AddEntity(string name, Vector2Int pos)
    {
        GameObject prefab = ResServer.instance.GetObject(name);
        if (prefab != null)
        {
            GameObject obj = Util.Inst(prefab, transform, pos);
            obj.GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
            if (obj.GetComponent<Entity>() != null)
                entityDict.Add(pos, obj.GetComponent<Entity>());
            //MapManager.instance.unitEntityDict.Add(new Vector2Int(x, y), obj.GetComponent<Entity>());
        }
        return prefab;
    }

    public void Refresh(Tilemap tileMap)
    {
        DeleteChilds();
        entityDict.Clear();
        for (int x = 0; x < tileMap.mapWidth; x++)
        {
            for (int y = 0; y < tileMap.mapHeight; y++)
            {
                //GameObject prefab = ResServer.instance.GetObject(tileMap.ground[x, y]);
                //if (prefab != null)
                //{
                //    GameObject obj = Util.Inst(prefab, transform, new Vector2Int(x, y));
                //    obj.GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
                //    if (obj.GetComponent<Entity>() != null)
                //        entityDict.Add(new Vector2Int(x, y), obj.GetComponent<Entity>());
                //    //MapManager.instance.groundEntityDict.Add(new Vector2Int(x, y), obj.GetComponent<Entity>());
                //}
                AddEntity(tileMap.ground[x, y], new Vector2Int(x, y));
            }
        }
    }

    public Entity CreateEntity(GameObject obj, Vector2Int pos)
    {
        GameObject newObj = Util.Inst(obj, transform, pos);
        return obj.GetComponent<Entity>();
    }

    /// <summary>
    /// 这个实现不好，以后会重写
    /// </summary>
    /// <param name="route"></param>
    public void RenderLine(LinkedList<Vector2Int> route)
    {
        if (route.Count > 0)
        {
            Vector3[] dots = new Vector3[route.Count];

            int i = 0;

            string s = "";

            foreach (var n in route)
            {
                dots[i++] = new Vector3(n.x + 1, n.y + 1);
                s += n.ToString();
            }

            lineRenderer.enabled = true;
            lineRenderer.positionCount = route.Count;
            lineRenderer.SetPositions(dots);
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

}
