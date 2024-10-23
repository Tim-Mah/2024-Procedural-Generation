using System.Collections.Generic;
using UnityEngine;

public class RectangularRoom : RoomGen
{
    protected override List<Vector2Int> GenerateRoom(BaseRoomParam parameters, Vector2Int startPosition)
    {
        List<Vector2Int> room = new List<Vector2Int>();
        int width = parameters.width;
        int height = parameters.height;

        // +3 will be minimun radius
        if (parameters.width - parameters.variation >= 3)
        {
            width -= (int)Random.Range(0f, (float)parameters.variation);
        }

        if (parameters.height - parameters.variation >= 3)
        {
            height -= (int)Random.Range(0f, (float)parameters.variation);
        }

        // Push start position to upper left corner of room
        int x = startPosition.x - (width / 2);
        int y = startPosition.y + (height / 2);

        Vector2Int position = new Vector2Int(x, y);

        room.Add(position);

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                room.Add(position);
                position += Vector2Int.right;
            }
            position.x = x;
            position += Vector2Int.down;
        }
        return room;
    }
}