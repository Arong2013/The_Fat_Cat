
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum RoomDirection
{
    Vertical,
    Horizontal
}

[System.Serializable]
public class Room
{
    public Vector2Int bottomLeft;
    public Vector2Int topRight;

    public Room parentRoom;
    public Room leftRoom;
    public Room rightRoom;

    private RoomDirection direction;
    int width => topRight.x - bottomLeft.x;
    int height => topRight.y - bottomLeft.y;

    public int Size => width * height;

    public Room(Vector2Int bottomLeft, Vector2Int topRight)
    {
        this.bottomLeft = bottomLeft;
        this.topRight = topRight;
    }

    public void SplitRoom(int splitRatio)
    {
            if (width > height)
        {
            direction = RoomDirection.Vertical;
            int splitX = Mathf.RoundToInt(bottomLeft.x + width * splitRatio / 100f);
            leftRoom = new Room(bottomLeft, new Vector2Int(splitX, topRight.y));
            rightRoom = new Room(new Vector2Int(splitX, bottomLeft.y), topRight);
        }
        else if (height > width)
        {
            direction = RoomDirection.Horizontal;
            int splitY = Mathf.RoundToInt(bottomLeft.y + height * splitRatio / 100f);
            leftRoom = new Room(bottomLeft, new Vector2Int(topRight.x, splitY));
            rightRoom = new Room(new Vector2Int(bottomLeft.x, splitY), topRight);
        }
        else // If the width and height are the same, randomly pick vertical or horizontal split
        {
            direction = Random.value > 0.5f ? RoomDirection.Vertical : RoomDirection.Horizontal;

            if (direction == RoomDirection.Vertical)
            {
                int splitX = Mathf.RoundToInt(bottomLeft.x + width * splitRatio / 100f);
                leftRoom = new Room(bottomLeft, new Vector2Int(splitX, topRight.y));
                rightRoom = new Room(new Vector2Int(splitX, bottomLeft.y), topRight);
            }
            else
            {
                int splitY = Mathf.RoundToInt(bottomLeft.y + height * splitRatio / 100f);
                leftRoom = new Room(bottomLeft, new Vector2Int(topRight.x, splitY));
                rightRoom = new Room(new Vector2Int(bottomLeft.x, splitY), topRight);
            }
        }

        leftRoom.parentRoom = this;
        rightRoom.parentRoom = this;
    }
}