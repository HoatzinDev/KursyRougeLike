using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public Room[] RoomPrefabs; // Префаби кімнат
    public int MaxRooms = 10; // Максимальна кількість кімнат
    public float RoomDistance = 5f; // Дистанція між кімнатами
    private List<Room> placedRooms = new List<Room>();

    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        Queue<Vector3> positionsToCheck = new Queue<Vector3>();

        // Стартова кімната
        Room firstRoom = Instantiate(RoomPrefabs[Random.Range(1, RoomPrefabs.Length)], Vector3.zero, Quaternion.identity);
        placedRooms.Add(firstRoom);
        positionsToCheck.Enqueue(Vector3.zero);

        while (placedRooms.Count < MaxRooms && positionsToCheck.Count > 0)
        {
            Vector3 currentPosition = positionsToCheck.Dequeue();
            Room currentRoom = GetRoomAtPosition(currentPosition);

            if (currentRoom == null) continue;

            for (int i = 0; i < 4; i++) // Перевіряємо всі 4 напрямки
            {
                Vector3 newPos = currentPosition + GetDirection(i);

                if (currentRoom.doors[i] && !IsRoomAtPosition(newPos))
                {
                    Room nextRoom = GetValidRoomForConnection(i);
                    if (nextRoom != null)
                    {
                        Room placedRoom = Instantiate(nextRoom, newPos, GetCorrectRotation(i));
                        placedRooms.Add(placedRoom);
                        positionsToCheck.Enqueue(newPos);
                    }
                }
            }
        }

        CloseEmptyConnections(); // Закриваємо порожні проходи глухими кутами
    }

    Room GetRoomAtPosition(Vector3 position)
    {
        foreach (Room room in placedRooms)
        {
            if (room.transform.position == position)
                return room;
        }
        return null;
    }

    bool IsRoomAtPosition(Vector3 position)
    {
        return GetRoomAtPosition(position) != null;
    }

    Room GetValidRoomForConnection(int direction)
    {
        List<Room> validRooms = new List<Room>();

        foreach (Room room in RoomPrefabs)
        {
            if (room.doors[(direction + 2) % 4]) // Перевіряємо, чи є вхід у правильному місці
                validRooms.Add(room);
        }

        return validRooms.Count > 0 ? validRooms[Random.Range(0, validRooms.Count)] : null;
    }

    Vector3 GetDirection(int index)
    {
        switch (index)
        {
            case 0: return new Vector3(-RoomDistance, 0, 0); // Вліво
            case 1: return new Vector3(RoomDistance, 0, 0);  // Вправо
            case 2: return new Vector3(0, 0, RoomDistance);  // Вперед
            case 3: return new Vector3(0, 0, -RoomDistance); // Назад
        }
        return Vector3.zero;
    }

    Quaternion GetCorrectRotation(int direction)
    {
        return Quaternion.Euler(0, direction * 90, 0);
    }

    void CloseEmptyConnections()
    {
       List<Vector3> emptyPositions = new List<Vector3>();

    // Перевіряємо, де є порожнечі
    foreach (Room room in placedRooms)
    {
        for (int i = 0; i < 4; i++)
        {
            Vector3 checkPos = room.transform.position + GetDirection(i);
            
            // Якщо тут немає кімнати, але є вихід – потрібно вставити нову кімнату
            if (room.doors[i] && !IsRoomAtPosition(checkPos))
            {
                emptyPositions.Add(checkPos);
            }
        }
    }

    // Аналізуємо порожні місця та вставляємо правильні кімнати
    foreach (Vector3 pos in emptyPositions)
    {
        int openDoors = CountOpenDoors(pos);
        RoomType chosenType = ChooseRoomType(openDoors, pos);
        Room newRoom = Instantiate(GetRoomPrefab(chosenType), pos, GetCorrectRotationForNewRoom(pos));
        placedRooms.Add(newRoom);
    }
}

int CountOpenDoors(Vector3 position)
{
    int count = 0;
    for (int i = 0; i < 4; i++)
    {
        if (IsRoomAtPosition(position + GetDirection(i))) count++;
    }
    return count;
}

RoomType ChooseRoomType(int openDoors, Vector3 position)
{
    switch (openDoors)
    {
        case 1: return RoomType.DeadEnd;
        case 2: return IsStraightPath(position) ? RoomType.Straight : RoomType.Corner;
        case 3: return RoomType.TShaped;
        case 4: return RoomType.Cross;
        default: return RoomType.DeadEnd;
    }
}

bool IsStraightPath(Vector3 position)
{
    return IsRoomAtPosition(position + GetDirection(0)) && IsRoomAtPosition(position + GetDirection(1)) ||
           IsRoomAtPosition(position + GetDirection(2)) && IsRoomAtPosition(position + GetDirection(3));
}

Room GetRoomPrefab(RoomType type)
{
    foreach (Room room in RoomPrefabs)
    {
        if (room.type == type) return room;
    }
    return RoomPrefabs[0]; // На випадок, якщо не знайдено
}

Quaternion GetCorrectRotationForNewRoom(Vector3 position)
{
    for (int i = 0; i < 4; i++)
    {
        if (IsRoomAtPosition(position + GetDirection(i)))
        {
            return Quaternion.Euler(0, i * 90, 0);
        }
    }
    return Quaternion.identity;

    }
}