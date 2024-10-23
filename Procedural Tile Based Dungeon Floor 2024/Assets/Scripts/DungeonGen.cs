using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.Linq;
using Unity.Mathematics;

public class DungeonGen : Generator
{
    //Dungeon Attributes

    [Tooltip("Room generator to use for generatring dungeon rooms.")]
    [SerializeField] protected RoomGen roomGenerator;

    [SerializeField] protected int dungeonRadius = 50;
    [SerializeField] protected int numRooms = 10;
    [SerializeField] protected int maxRoomRadius = 7;

    [Tooltip("0.5 makes path randomly, 1 more square")]
    [SerializeField] [Range(0.5f, 1f)] private float pathSquaring = 0.5f;

    [Tooltip("Smooths out rooms and paths (Walls are just outlines and always have void next to them)")]
    [SerializeField] private bool smoothing = true;

    private static Vector2Int playerSpawn = Vector2Int.zero;

    protected List<Vector2Int> DungeonLayout(int numRooms)
    {
        List<Vector2Int> dungeonSpawnPoint = new List<Vector2Int>();
        float pi = (float) Math.PI;
        float newRad = (float) Math.Pow(dungeonRadius, 2);

        int i = 0; //Don't want this to inf if rooms too big for area
        while(dungeonSpawnPoint.Count < numRooms && i < 100)
        {
            i++;
            // Even distribution inside circle
            float x = (float)(Math.Sqrt(Random.Range(0, newRad)) * Math.Cos(Random.Range(0,2 * pi)));
            float y = (float)(Math.Sqrt(Random.Range(0, newRad)) * Math.Sin(Random.Range(0, 2 * pi)));
            
            Vector2Int room = new Vector2Int((int)Math.Floor(x), (int)Math.Floor(y));
            bool canGen = true;

            // Verify a dungeon can fit in random space
            foreach (var point in dungeonSpawnPoint)
            {
                //Check if distance to current dungeon is big enough
                if (Vector2.Distance(room, point) < maxRoomRadius * 2)
                {
                    canGen = false;
                    break;
                }
            }
            if (canGen)
            {
                dungeonSpawnPoint.Add(room);
            }
        }

        return dungeonSpawnPoint;
    }

    protected List<Vector2Int> Connect(Vector2Int room1, Vector2Int room2)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        path.Add(room1);
        // Chance next step on path is same direction as last

        Vector2Int position = room1;

        while (position != room2)
        {
            while(Random.Range(0.0f, 1.0f) <= pathSquaring && position.x != room2.x)
            {
                //Move path horizontal
                if (room2.x > position.x)
                {
                    position += Vector2Int.right;
                }
                else
                {
                    position += Vector2Int.left;
                }
                path.Add(position);
            }
            float yWeight = 1 - pathSquaring;
            if(pathSquaring == 1)
            {
                //If weight is one, we always want it to contrinue, if it can. So
                //once x is done, we need y to always run.
                yWeight = 1;
            }
            while (Random.Range(0.0f, 1.0f) <= yWeight && position.y != room2.y)
            {
                //Move path vertical
                if (room2.y > position.y)
                {
                    position += Vector2Int.up;
                }
                else
                {
                    position += Vector2Int.down;
                }
                path.Add(position);
            }
        }
        return path;
    }

    protected Vector2Int Closest(Vector2Int targetRoom, List<Vector2Int> roomList)
    {
        float closestDist = float.MaxValue;
        Vector2Int closest = Vector2Int.zero;
        foreach (var room in roomList)
        {
            if(targetRoom != room)
            {
                float checkDistance = Vector2.Distance(targetRoom, room);
                if (checkDistance < closestDist)
                {
                    closestDist = checkDistance;
                    closest = room;
                }
            }
        }
        return closest;
    }

    private List<List<Vector2Int>> AssembleDungeon(List<Vector2Int> dungeonPoints)
    {
        Vector2Int prevRoom = Vector2Int.zero;
        Vector2Int currRoom = dungeonPoints[Random.Range(0, dungeonPoints.Count)];

        List<Vector2Int> roomsPlan = roomGenerator.GetRoomPlan(currRoom);
        //Centre point will be on path anyways
        List<Vector2Int> pathsPlan = new List<Vector2Int>();
        pathsPlan.Add(currRoom);

        //Connects each room to it's closest room (will not create connected graph)
        foreach (var point in dungeonPoints)
        {
            roomsPlan = roomsPlan.Union(roomGenerator.GetRoomPlan(point)).ToList<Vector2Int>();
            pathsPlan = pathsPlan.Union(Connect(point, Closest(point, dungeonPoints))).ToList<Vector2Int>();
        }

        //Connects closest reamaing room, then removes current room from list.
        //Creates connected graph, but no options for the player (too linear)
        while(dungeonPoints.Count > 1)
        {
            prevRoom = currRoom;
            dungeonPoints.Remove(prevRoom);
            currRoom = Closest(prevRoom, dungeonPoints);

            //roomsPlan = roomsPlan.Union(room.GetRoomPlan(currRoom)).ToList<Vector2Int>();
            pathsPlan = pathsPlan.Union(Connect(prevRoom, currRoom)).ToList<Vector2Int>();
        }

        return new List<List<Vector2Int>> {roomsPlan, pathsPlan};
    }

    private List<List<Vector2Int>> SmoothDungeon(List<List<Vector2Int>> floorPlan)
    {
        if (floorPlan.Count != 2)
        {
            throw new Exception("floorPlan must be a list of 2 Vector2Int lists (roomsPlan and pathsPlan)");
        }
        //This adds walls to smooth everything, then removes only the perimeter.
        //Makes sure the default ruleTiles works and no invisible walls
        List<Vector2Int> fullDungeon = floorPlan[0];

        fullDungeon = fullDungeon.Union(floorPlan[1]).ToList();
        fullDungeon = fullDungeon.Union(WallScript.CreateWalls(fullDungeon)).ToList();

        //Remove all perimeter tiles
        List<Vector2Int> wallPlan = new List<Vector2Int>();

        foreach (var tile in fullDungeon)
        {
            foreach (var direction in Direction2D.eightDirectionList)
            {
                if(!fullDungeon.Contains(tile + direction))
                {
                    wallPlan.Add(tile);
                }
            }
        }

        wallPlan = wallPlan.Distinct().ToList();
        foreach (var tile in wallPlan)
        {
            fullDungeon.Remove(tile);
        }

        //Separate out rooms and paths list. Added tiles will go to the paths list
        List<Vector2Int> newPathsList = new List<Vector2Int>();
        foreach (var tile in fullDungeon)
        {
            if (!floorPlan[0].Contains(tile))
            {
                newPathsList.Add(tile);
            }
        }

        return new List<List<Vector2Int>> {floorPlan[0], newPathsList, wallPlan};
    }

    public override void CreateTilePlan()
    {
        //Plan has both a list of paths and rooms
        List<List<Vector2Int>> plan = null;
        List<Vector2Int> combinedPlan = null;
        List<Vector2Int> wallPlan = null;
        if (smoothing)
        {
            plan = SmoothDungeon(AssembleDungeon(DungeonLayout(numRooms)));
            //combinedPlan = plan[0].Union(plan[1]).ToList().Union(plan[2]).ToList();
            combinedPlan = plan[0].Union(plan[1]).ToList();

            //This is really to visualize the wall plan
            wallPlan = plan[2];

            tileMap.Clear();
            tileMap.PaintTile(combinedPlan, wallPlan);
        }
        else
        {
            plan = AssembleDungeon(DungeonLayout(numRooms));
            combinedPlan = plan[0].Union(plan[1]).ToList();
            wallPlan = WallScript.CreateWalls(combinedPlan);
            //combinedPlan = combinedPlan.Union(wallPlan).ToList();

            tileMap.Clear();
            tileMap.PaintTile(combinedPlan, wallPlan);
        }
        playerSpawn = plan[0][Random.Range(0, plan[0].Count)];
    }
    public Vector2Int GetSpawnPoint()
    {
        return playerSpawn;
    }

}
