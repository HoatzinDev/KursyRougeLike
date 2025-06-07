using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDebugger : MonoBehaviour
{
    public Room room; // Вказуємо кімнату, яку перевіряємо
    public float debugOffset = 2.5f; // Половина RoomSize для розташування точок
    public GameObject debugPointPrefab; // Префаб точки для позначення дверей

    void Start()
    {
        DebugRoomConnections();
    }

    void DebugRoomConnections()
    {
        string[] directions = { "Left", "Right", "Forward", "Backward" };

        for (int i = 0; i < 4; i++)
        {
            if (room.doors[i]) // Перевіряємо, чи двері в цьому напрямку є
            {
                Vector3 pos = room.transform.position + GetDirection(i) * debugOffset;
                GameObject point = Instantiate(debugPointPrefab, pos, Quaternion.identity);
                point.name = directions[i]; // Підписуємо мітку напрямком
            }
        }
    }

    Vector3 GetDirection(int index)
    {
        switch (index)
        {
            case 0: return new Vector3(-1, 0, 0); // Вліво
            case 1: return new Vector3(1, 0, 0);  // Вправо
            case 2: return new Vector3(0, 0, 1);  // Вперед
            case 3: return new Vector3(0, 0, -1); // Назад
        }
        return Vector3.zero;
    }

}
