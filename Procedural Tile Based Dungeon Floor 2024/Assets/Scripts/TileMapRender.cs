using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapRender : MonoBehaviour
{
    [SerializeField] private Tilemap floorMap;
    [SerializeField] private TileBase ruleTile;

    [SerializeField] private Tilemap wallMap;
    [SerializeField] private TileBase wallTile;

    public void Clear()
    {
        floorMap.ClearAllTiles();
        wallMap.ClearAllTiles();
    }

    public void PaintTile(IEnumerable<Vector2Int> floorPlan, IEnumerable<Vector2Int> wallPlan)
    {
        foreach (var position in floorPlan)
        {
            var tilePos = floorMap.WorldToCell((Vector3Int) position);
            floorMap.SetTile(tilePos, ruleTile);
        }

        foreach (var position in wallPlan)
        {
            var tilePos = floorMap.WorldToCell((Vector3Int)position);
            wallMap.SetTile(tilePos, wallTile);
        }
    }

    //Paint singles
    //Paint Corners
}
