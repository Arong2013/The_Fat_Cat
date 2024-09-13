using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
public struct Edge
{
    public Vector2 p1, p2;

    public Edge(Vector2 p1, Vector2 p2)
    {
        this.p1 = p1;
        this.p2 = p2;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is Edge)) return false;
        Edge other = (Edge)obj;
        // Check if the edges are equal (considering both directions)
        return (p1 == other.p1 && p2 == other.p2) || (p1 == other.p2 && p2 == other.p1);
    }

    public override int GetHashCode()
    {
        // Combine hash codes of the points
        return p1.GetHashCode() ^ p2.GetHashCode();
    }
}

public class DungeonMaker : SerializedMonoBehaviour
{
    [Title("Room Size Settings")]
    [MinValue(1)]
    [LabelText("Min Width")]
    public int minWidth = 5;

    [MinValue(1)]
    [LabelText("Min Height")]
    public int minHeight = 5;

    [MinValue(1)]
    [LabelText("Dungeon Size")]
    public int dungeonSize = 10;

    [Title("Room Count Settings")]
    [MinMaxSlider(1, 100, true)]
    [LabelText("Room Count Range")]
    public Vector2Int roomCountRange = new Vector2Int(1, 5);

    [Title("Visualization Settings")]
    [ColorPalette("RoomColors")]
    [LabelText("Room Color")]
    public Color roomColor = Color.green;

    [ColorPalette("RoomColors")]
    [LabelText("Circle Color")]
    public Color circleColor = Color.red;

    [Range(0.1f, 2f)]
    [LabelText("Circle Radius")]
    public float circleRadius = 0.5f;

    [Range(10f, 500f)]
    [LabelText("Min Distance Between Rooms")]
    public float minDistanceBetweenRooms = 100f;

    [ShowInInspector, ReadOnly]
    [LabelText("Room List")]
    private List<Room> roomList = new List<Room>();

    [ShowInInspector, ReadOnly]
    [LabelText("Selected Rooms")]
    private List<Room> selectedRooms = new List<Room>();

    [Button("Generate Dungeon")]
    public void Start()
    {
        GenerateDungeon();
    }

    public void GenerateDungeon()
    {
        roomList.Clear();
        selectedRooms.Clear();
        GenerateMap();
        selectedRooms = GetNonOverlappingRandomRooms(roomCountRange.x, roomCountRange.y);
    }

    public void GenerateMap()
    {
        Room room = new Room(new Vector2Int(0, 0), new Vector2Int(dungeonSize, dungeonSize));
        CutRoom(room);
    }

    void CutRoom(Room _room)
    {
        int dividedRatio = Random.Range(30, 71);
        if (_room.Size <= minWidth * minHeight * 1.5)
        {
            roomList.Add(_room);
        }
        else
        {
            _room.SplitRoom(dividedRatio);
            CutRoom(_room.leftRoom);
            CutRoom(_room.rightRoom);
        }
    }

    List<Room> GetNonOverlappingRandomRooms(int minRooms, int maxRooms)
    {
        maxRooms = Mathf.Min(maxRooms, roomList.Count);
        int numRoomsToSelect = Random.Range(minRooms, maxRooms + 1);
        float currentMinDistance = minDistanceBetweenRooms;
        List<Room> selectedRooms = new List<Room>();
        for (int attempts = 0; attempts < 1000; attempts++)
        {
            selectedRooms.Clear();
            for (int roomSelectionAttempts = 0; roomSelectionAttempts < 1000 && selectedRooms.Count < numRoomsToSelect; roomSelectionAttempts++)
            {
                Room candidateRoom = roomList[Random.Range(0, roomList.Count)];
                Vector2 candidateCenter = ((Vector2)candidateRoom.bottomLeft + (Vector2)candidateRoom.topRight) / 2f;

                if (!selectedRooms.Exists(selectedRoom =>
                    Vector2.Distance(candidateCenter, ((Vector2)selectedRoom.bottomLeft + (Vector2)selectedRoom.topRight) / 2f) < currentMinDistance))
                {
                    selectedRooms.Add(candidateRoom);
                }
            }
            if (selectedRooms.Count >= numRoomsToSelect)
                break;
            currentMinDistance *= 0.9f;
        }
        print(currentMinDistance);
        return selectedRooms;
    }

    void OnDrawGizmos()
    {
        if (roomList == null || roomList.Count == 0)
            return;

        // Draw rooms and their centers
        foreach (Room room in roomList)
        {
            Vector2 roomCenter = ((Vector2)room.bottomLeft + (Vector2)room.topRight) / 2f;
            Vector2 roomSize = room.topRight - room.bottomLeft;

            // Default room color
            Gizmos.color = roomColor;
            Gizmos.DrawWireCube(new Vector3(roomCenter.x, 0, roomCenter.y), new Vector3(roomSize.x, 1, roomSize.y));
        }

        // Draw spheres for the selected rooms
        Gizmos.color = Color.blue;
        foreach (Room room in selectedRooms)
        {
            Vector2 roomCenter = ((Vector2)room.bottomLeft + (Vector2)room.topRight) / 2f;
            Gizmos.DrawSphere(new Vector3(roomCenter.x, 0, roomCenter.y), circleRadius);
        }

        // Draw lines connecting the selected rooms in an optimal path
        if (selectedRooms.Count > 1)
        {
            Gizmos.color = Color.yellow;
            List<Room> path = FindOptimalPath(selectedRooms);

            // Detect intersecting rooms with the path
            HashSet<Room> intersectingRooms = new HashSet<Room>();

            for (int i = 0; i < path.Count - 1; i++)
            {
                Vector2 start = ((Vector2)path[i].bottomLeft + (Vector2)path[i].topRight) / 2f;
                Vector2 end = ((Vector2)path[i + 1].bottomLeft + (Vector2)path[i + 1].topRight) / 2f;

                // Draw the path line
                Gizmos.DrawLine(new Vector3(start.x, 0, start.y), new Vector3(end.x, 0, end.y));

                // Check intersecting rooms
                foreach (Room room in roomList)
                {
                    Vector2 roomCenter = ((Vector2)room.bottomLeft + (Vector2)room.topRight) / 2f;
                    Vector2 roomSize = room.topRight - room.bottomLeft;
                    Vector2[] roomCorners = {
                    room.bottomLeft,
                    new Vector2(room.bottomLeft.x, room.topRight.y),
                    room.topRight,
                    new Vector2(room.topRight.x, room.bottomLeft.y)
                };

                    bool intersects = false;
                    for (int j = 0; j < roomCorners.Length; j++)
                    {
                        Vector2 startCorner = roomCorners[j];
                        Vector2 endCorner = roomCorners[(j + 1) % roomCorners.Length];
                        if (DoEdgesIntersect(new Edge(start, end), new Edge(startCorner, endCorner)))
                        {
                            intersects = true;
                            break;
                        }
                    }

                    if (intersects)
                    {
                        intersectingRooms.Add(room);
                    }
                }
            }

            // Draw intersecting rooms in red
            Gizmos.color = Color.red;
            foreach (Room room in intersectingRooms)
            {
                Vector2 roomCenter = ((Vector2)room.bottomLeft + (Vector2)room.topRight) / 2f;
                Vector2 roomSize = room.topRight - room.bottomLeft;
                Gizmos.DrawWireCube(new Vector3(roomCenter.x, 0, roomCenter.y), new Vector3(roomSize.x, 1, roomSize.y));
            }
        }
    }


    bool IsRoomIntersectingWithLine(Room room, Vector2 lineStart, Vector2 lineEnd)
    {
        // Room boundaries as edges
        Edge[] roomEdges = new Edge[]
        {
        new Edge(room.bottomLeft, new Vector2(room.topRight.x, room.bottomLeft.y)), // bottom edge
        new Edge(room.bottomLeft, new Vector2(room.bottomLeft.x, room.topRight.y)), // left edge
        new Edge(new Vector2(room.topRight.x, room.bottomLeft.y), room.topRight),   // right edge
        new Edge(new Vector2(room.bottomLeft.x, room.topRight.y), room.topRight)    // top edge
        };

        // Check if the line intersects with any of the room edges
        foreach (var roomEdge in roomEdges)
        {
            if (DoEdgesIntersect(roomEdge, new Edge(lineStart, lineEnd)))
            {
                return true;
            }
        }

        return false;
    }

    bool DoEdgesIntersect(Edge edge1, Edge edge2)
    {
        return AreLinesIntersecting(edge1.p1, edge1.p2, edge2.p1, edge2.p2);
    }

    // Helper method to check if two line segments intersect
    bool AreLinesIntersecting(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
    {
        float denominator = (p4.y - p3.y) * (p2.x - p1.x) - (p4.x - p3.x) * (p2.y - p1.y);
        if (denominator == 0f) return false; // Parallel lines

        float ua = ((p4.x - p3.x) * (p1.y - p3.y) - (p4.y - p3.y) * (p1.x - p3.x)) / denominator;
        float ub = ((p2.x - p1.x) * (p1.y - p3.y) - (p2.y - p1.y) * (p1.x - p3.x)) / denominator;

        // Check if the intersection is within the bounds of both line segments
        return ua >= 0f && ua <= 1f && ub >= 0f && ub <= 1f;
    }

    List<Room> FindOptimalPath(List<Room> rooms)
    {
        List<Room> path = new List<Room>();
        HashSet<Room> visited = new HashSet<Room>();
        Room current = rooms[0];
        path.Add(current);
        visited.Add(current);

        while (path.Count < rooms.Count)
        {
            Room next = null;
            float minDistance = float.MaxValue;

            foreach (Room room in rooms)
            {
                if (!visited.Contains(room))
                {
                    float distance = Vector2.Distance(((Vector2)current.bottomLeft + (Vector2)current.topRight) / 2f, ((Vector2)room.bottomLeft + (Vector2)room.topRight) / 2f);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        next = room;
                    }
                }
            }

            if (next != null)
            {
                path.Add(next);
                visited.Add(next);
                current = next;
            }
        }

        return path;
    }

}
