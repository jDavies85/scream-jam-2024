using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public DungeonTile[] tiles = new DungeonTile[6];
    public GameObject entrancesAndExits;
    public bool generateRandomMap;
    private List<GameObject> deck = new List<GameObject>();
    private List<GameObject> placedTiles = new List<GameObject>();
    private Room lastRoom = null;
    private int deckIndex = 1;
    void Awake()
    {
        if(generateRandomMap)
        {
            GenerateDeck();
            lastRoom = deck[0].GetComponent<Room>();
            lastRoom.gameObject.SetActive(true);
            var coroutine = WaitAndPlace(.05f);
            StartCoroutine(coroutine);

            //for (int i = 1; i < deck.Count; i++)
            //{
            //    PlaceTile();
            //}
        }
    }

    private IEnumerator WaitAndPlace(float waitTime)
    {
        for (int i = 1; i < deck.Count; i++)
        {
            PlaceTile();
            yield return new WaitForSeconds(waitTime);
        }
    }

    public void GenerateDungeon()
    {
        var entrance = deck.First();
        var exit = deck.Last();
        var tmpList = deck.GetRange(1, deck.Count - 2).ToList();
        deck.Clear();
        deck.Add(entrance);
        deck.AddRange(tmpList);
        deck.Add(exit); 
        placedTiles.Clear();
        lastRoom = null;
        deckIndex = 1;
        lastRoom = deck[0].GetComponent<Room>();
        for (int i = 1; i < deck.Count; i++)
        {
            PlaceTile();
        }
    }

    public void PlaceTile()
    {
        if(deckIndex < deck.Count)
        {
            var tile = deck[deckIndex];
            placedTiles.Add(tile);
            var currentRoom = tile.GetComponent<Room>();
            
            TryPlaceTile(tile);
            
            lastRoom = currentRoom;
            deckIndex++;
        }
    }

    private void TryPlaceTile(GameObject tile, int count = 0)
    {
        if (count == 15)
            throw new System.InvalidOperationException("Too much recursion");

        var currentRoom = tile.GetComponent<Room>();

        var validSpawners = lastRoom.roomSpawners.Where(x => !x.isOccupied).ToList();

        if (!validSpawners.Any() || count == 10)
        {
            validSpawners = FindNewTile(count);
        }

        int rand = Random.Range(0, validSpawners.Count);
        var spawner = validSpawners[rand];
        
        tile.transform.position = spawner.gameObject.transform.position;
        tile.SetActive(true);
        foreach (var item in currentRoom.roomSpawners)
        {
            item.isOccupied = false;
        }
        var isValidPlacement = false;
        //rotate up to 4 times
        for (int i = 0; i < 4; i++)
        {
            isValidPlacement = currentRoom.IsValidPlacement(i);
            if (!isValidPlacement)
                currentRoom.RotateRoom();
            else
                break;
        }
        //if still not valid room, need to find a new current room
        if (!isValidPlacement)
        {
            count++;
            TryPlaceTile(tile, count);
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
                deck.Add(Instantiate(tiles[i].prefab));
            }
        }
        deck.Shuffle();
        var tmpList = deck.ToList();
        deck.Clear();
        deck.Add(Instantiate(entrancesAndExits));
        deck.AddRange(tmpList);
        deck.Add(Instantiate(entrancesAndExits));
        foreach(var tile in deck)
        {
            tile.SetActive(false);
            foreach (var item in tile.GetComponent<Room>().roomSpawners)
            {
                item.isOccupied = false;
            }
        }
    }
}

[System.Serializable]
public class DungeonTile
{
    public GameObject prefab;
    public int count;
}

public static class IListExtensions
{
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}
