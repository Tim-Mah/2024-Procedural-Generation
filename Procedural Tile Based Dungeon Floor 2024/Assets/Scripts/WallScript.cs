using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WallScript : MonoBehaviour
{
    public static List<Vector2Int> CreateWalls(List<Vector2Int> floorPlan)
    {
        List<Vector2Int> wallPlan = new List<Vector2Int>();
        //Use the floorplan to find edges, then place walls there and store in <walls>
        foreach (var tile in floorPlan)
        {
            //Check every tile around it in 8 directions. if it is empty, it's a wall
            foreach (var direction in Direction2D.eightDirectionList)
            {
                //If it doesn't contain a tile in "direction"
                if (!floorPlan.Contains(tile + direction))
                {
                    wallPlan.Add(tile + direction);
                }
            }
        }
        //Remove duplicates
        return wallPlan.Distinct().ToList();
    }

    //public static List<String> TileInfo(List<Vector2Int> floorPlan, List<Vector2Int> wallPlan)
    //For each wall tile, figure out the correct string to represent neighbours
    //Returns list of the strings
}

