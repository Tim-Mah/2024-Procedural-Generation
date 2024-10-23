using System.Collections.Generic;
using UnityEngine;

public class CircularRoom : RoomGen
{
    // Very simple circle generation algorithm. Checks if a point in the bounding box is
    // Less than the circle's radius
    protected override List<Vector2Int> GenerateRoom(BaseRoomParam parameters, Vector2Int startPosition)
    {
        List<Vector2Int> room = new List<Vector2Int>();

        int radius = parameters.radius;

        // +3 will be minimun radius
        if(parameters.radius - parameters.variation >= 3)
        {
            radius -= (int) Random.Range(0f, (float) parameters.variation);
        }

        //Bounding box square for the citcle
        int width = radius * 2;

        // Push start position to upper left corner of room
        int x = startPosition.x - radius;
        int y = startPosition.y + radius;

        Vector2Int position = new Vector2Int(x, y);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (Vector2.Distance(position, startPosition) < radius)
                {
                    room.Add(position);
                }
                position += Vector2Int.right;
            }
            position.x = x;
            position += Vector2Int.down;
        }
        return room;
    }
}