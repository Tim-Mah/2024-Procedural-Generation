using System.Collections.Generic;
using UnityEngine;

public abstract class RoomGen : Generator
{
    [Tooltip("The parameters for generating a room")]
    [SerializeField] protected BaseRoomParam roomParameters;

    public override void CreateTilePlan()
    {
        List<Vector2Int> floorPlan = GetRoomPlan();
        tileMap.Clear();
        List<Vector2Int> wallPlan = WallScript.CreateWalls(floorPlan);
        tileMap.PaintTile(floorPlan, wallPlan);
    }

    public List<Vector2Int> GetRoomPlan()
    {
        return GenerateRoom(roomParameters, Vector2Int.zero);
    }

    public List<Vector2Int> GetRoomPlan(Vector2Int position)
    {
        return GenerateRoom(roomParameters, position);
    }

    public void SetRoomStyle(BaseRoomParam room)
    {
        roomParameters = room;
    }

    protected abstract List<Vector2Int> GenerateRoom(BaseRoomParam parameters, Vector2Int startPosition);
}

public static class Direction2D
{
    public static List<Vector2Int> cardDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(0, 1), //Up
        new Vector2Int(1, 0), //Right
        new Vector2Int(0, -1), //Down
        new Vector2Int (-1, 0) //Left
    };

    public static List<Vector2Int> diagDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(1, 1), //Up Right
        new Vector2Int(1, -1), //Down Right
        new Vector2Int(-1, -1), //Down Left
        new Vector2Int (-1, 1) //Up Left
    };

    public static List<Vector2Int> eightDirectionList = new List<Vector2Int>
    {
        new Vector2Int(0, 1), //Up
        new Vector2Int(1, 1), //Up Right
        new Vector2Int(1, 0), //Right
        new Vector2Int(1, -1), //Down Right
        new Vector2Int(0, -1), //Down
        new Vector2Int(-1, -1), //Down Left
        new Vector2Int (-1, 0), //Left
        new Vector2Int (-1, 1) //Up Left
    };

    public static Vector2Int GetRandDirection()
    {
        return cardDirectionsList[Random.Range(0, cardDirectionsList.Count)];
    }
}