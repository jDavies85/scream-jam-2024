using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject[] walls;
    public GameObject[] spawnpoints;
    public List<GameObject> neighbours;
    public RoomSpawner[] roomSpawners;

    private void Awake()
    {
        roomSpawners = new RoomSpawner[spawnpoints.Length];
        for (int i = 0; i < roomSpawners.Length; i++)
        {
            roomSpawners[i] = new RoomSpawner(spawnpoints[i]);
        }
    }
}

[System.Serializable]
public class RoomSpawner
{
    public GameObject spawnpoint;
    public bool isOccupied;

    public RoomSpawner(GameObject sp)
    {
        spawnpoint = sp;
    }
}
