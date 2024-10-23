using System.Collections.Generic;
using UnityEngine;

public class RandomWalkRoom : RoomGen
{
    protected override List<Vector2Int> GenerateRoom(BaseRoomParam parameters, Vector2Int startPosition)
    {
        List<Vector2Int> room = new List<Vector2Int>();
        Vector2Int position = startPosition;
        room.Add(position);

        // Randomly choose to move to an adjacent direction
        for (int i = 0; i < parameters.iterations; i++)
        {
            position = startPosition;
            for (int j = 0; j < parameters.walkLength; j++)
            {
                //Add a vector to move one space in cardinal directions
                Vector2Int nextStep = position + Direction2D.GetRandDirection();
                if (!room.Contains(nextStep))
                {
                    room.Add(nextStep);
                }
                position = nextStep;
            }
        }
        return room;
    }
}