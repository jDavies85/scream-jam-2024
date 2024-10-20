using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public DungeonTile[] tiles = new DungeonTile[6];
    public GameObject entrancesAndExits;
    public bool isRandomDeck;
    public bool generateDungeon;
    private List<GameObject> deck = new List<GameObject>();
    private List<GameObject> placedTiles = new List<GameObject>();
    private List<RoomSpawner> spawnpoints = new List<RoomSpawner>();
    private Room lastRoom = null;
    private int deckIndex = 1;

    private void Awake()
    {
        if(generateDungeon)
        {
            GenerateDeck();
            lastRoom = deck[0].GetComponent<Room>();
            placedTiles.Add(deck[0]);
            GenerateDungeon();
            //foreach (var currentRoom in placedTiles.Select(x => x.GetComponent<Room>()).ToList())
            //{
            //    Debug.Log("Placing Doors");
            //    currentRoom.CheckNeighbours();
            //}
        }
    }
    public void GenerateDungeon()
    {
        int count = 0;
        for (int i = 1; i < deck.Count; i++)
        {
            if (deckIndex < deck.Count)
            {
                var tile = deck[deckIndex];

                var currentRoom = tile.GetComponent<Room>();
                spawnpoints.AddRange(currentRoom.roomSpawners);
                //foreach (var rs in spawnpoints)
                //{
                //    if (placedTiles.Any(x => Vector3.Distance(rs.transform.position, x.transform.position) < 0.05f))
                //    {
                //        Debug.Log("Marking " + rs + " as occupied");
                //        rs.isOccupied = true;
                //    }
                //}
                ReconcileTilesAndSpawnpoints();
                //Do the work here
                var validSpawners = lastRoom.roomSpawners.Where(x => !x.isOccupied).ToList();

                if (!validSpawners.Any() || count == 10)
                {
                    validSpawners = FindNewTile(count);
                }

                int rand = Random.Range(0, validSpawners.Count);
                var spawner = validSpawners[rand];
                tile.transform.position = spawner.gameObject.transform.position;
                for (int j = 0; j < 4; j++)
                {
                    bool isValidPlacement = currentRoom.IsValidPlacementV2(currentRoom, spawner.connector);
                    if (!isValidPlacement)
                    {
                        currentRoom.RotateRoom();
                    }
                }
                spawner.isOccupied = true;
                placedTiles.Add(tile);
                lastRoom.neighbours.Add(currentRoom.gameObject);
                currentRoom.neighbours.Add(lastRoom.gameObject);
                lastRoom = currentRoom;
                deckIndex++;
            }
        }
    }

    public List<RoomSpawner> FindNewTile(int index = 0)
    {
        Debug.Log("There are no valid placements for this tile, find another origin tile.");
        var rooms = placedTiles.Select(x => x.GetComponent<Room>()).Where(x => x.roomSpawners.Any(x => !x.isOccupied)).ToList();
        if (index >= rooms.Count)
            throw new System.InvalidOperationException("Tried all the rooms");
        lastRoom = rooms[index];
        if (lastRoom == null)
            Debug.Log("Things are really bad!");
        return lastRoom.roomSpawners.Where(x => !x.isOccupied).ToList();
    }

    void GenerateDeck()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < tiles[i].count; j++)
            {
                deck.Add(Instantiate(tiles[i].prefab, new Vector3(15* j, 0, 15*j), transform.rotation));
            }
        }
        if(isRandomDeck)
        {
            deck.Shuffle();
        }
        var tmpList = deck.ToList();
        deck.Clear();
        deck.Add(Instantiate(entrancesAndExits));
        deck.AddRange(tmpList);
        deck.Add(Instantiate(entrancesAndExits, new Vector3(5 * 15, 0, 5 * 15), transform.rotation));
    }
    private void ReconcileTilesAndSpawnpoints(float tolerance = 0.05f)
    {

        // Check list1 against list2
        foreach (var spawnpoint in spawnpoints)
        {
            bool isMatching = false;
            foreach (GameObject tile in placedTiles)
            {
                if (Vector3.Distance(spawnpoint.transform.position, tile.transform.position) < tolerance)
                {
                    isMatching = true;
                    spawnpoint.isOccupied = true;
                    break;
                }
            }

            if (!isMatching)
            {
                spawnpoint.isOccupied = false;
            }
        }
    }
}
