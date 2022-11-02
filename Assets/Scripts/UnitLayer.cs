using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitLayer : MonoBehaviour
{
    public static UnitLayer instance { get; private set; }

    private void Awake()
    {
        instance = this;
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
            obj.GetComponent<SpriteRenderer>().sortingLayerName = "Unit";
            if (obj.GetComponent<Entity>() != null)
                entityDict.Add(pos, obj.GetComponent<Entity>());
            //MapManager.instance.unitEntityDict.Add(new Vector2Int(x, y), obj.GetComponent<Entity>());
        }
        return prefab;
    }

    public void Refresh(Tilemap tileMap)
    {
        DeleteChilds();

        for (int x = 0; x < tileMap.mapWidth; x++)
        { 
            for (int y = 0; y < tileMap.mapHeight; y++)
            {
                //GameObject prefab = ResServer.instance.GetObject(tileMap.unit[x, y]);
                //if (prefab != null)
                //{
                //    GameObject obj = Util.Inst(prefab, transform, new Vector2Int(x, y));
                //    obj.GetComponent<SpriteRenderer>().sortingLayerName = "Unit";
                //    if (obj.GetComponent<Entity>() != null)
                //        entityDict.Add(new Vector2Int(x, y), obj.GetComponent<Entity>());
                //    //MapManager.instance.unitEntityDict.Add(new Vector2Int(x, y), obj.GetComponent<Entity>());
                //}
                AddEntity(tileMap.unit[x, y], new Vector2Int(x, y));
            }
        }
    }
}
