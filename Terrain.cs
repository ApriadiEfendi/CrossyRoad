using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : MonoBehaviour
{
    [SerializeField] GameObject tilePrefab;

    private void Start() {
        Generate(9);
    }

    protected int horizontalSize;

    public virtual void Generate(int size)
    {
        horizontalSize = size;
        
        if(size == 0)
            return;

        if((float)size % 2 == 0)
            size -= 1;

        int moveLimit = Mathf.FloorToInt((float) size / 2);

        for (int i = -moveLimit; i <= moveLimit ; i++)
        {
            Spawntile(i);
        }

        var leftBoundaryTile = Spawntile(-moveLimit -1);
        var rightBoundaryTile = Spawntile(moveLimit +1);

        DarkenObject(leftBoundaryTile);
        DarkenObject(rightBoundaryTile);
    
    }
    private GameObject Spawntile(int xPos)
    {
         var go = Instantiate(
                tilePrefab,
                transform);
            go.transform.localPosition = new Vector3(xPos,0,0);

        return go;
    }

    private void DarkenObject(GameObject go)
    {
        var renderers = go.GetComponentsInChildren<MeshRenderer>(includeInactive: true);

        foreach (var rend in renderers)
        {
            rend.material.color = rend.material.color * Color.grey;
        }
    }
}
