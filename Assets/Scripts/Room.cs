using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum RoomType
{
    DeadEnd,    // [0] Глухий кут (1 вхід)
    Straight,   // [1] Прямий прохід (2 входи)
    Corner,     // [2] Г-подібний (2 входи)
    TShaped,    // [3] Т-подібний (3 входи)
    Cross       // [4] X-подібний (4 входи)
}

public class Room : MonoBehaviour
{
    public RoomType type;
    public GameObject prefab;
    public bool[] doors = new bool[4]; // Входи/виходи на 4 сторони (0 = Вліво, 1 = Вправо, 2 = Вперед, 3 = Назад)
}

