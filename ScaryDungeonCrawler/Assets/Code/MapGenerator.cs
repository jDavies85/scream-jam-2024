using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

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
            //for (int i = 1; i < deck.Count; i++)
            //{
            //    PlaceTile();
            //}
            //var coords = new Vector3();
            //int rowCount = 4;
            //for (int i = 0; i < deck.Count; i++)
            //{
            //    deck[i].transform.position = coords;
            //    coords.x += 3;
            //    rowCount--;
            //    if(rowCount == 0)
            //    {
            //        coords.x = 0;
            //        coords.z += 3;
            //        rowCount = 4;
            //    }
            //}
        }
    }

    bool IsValidPlacement()
    {
        return true;
    }

    public void PlaceTile()
    {
        if(deckIndex < deck.Count)
        {
            var tile = deck[deckIndex];
            placedTiles.Add(tile);
            var currentRoom = tile.GetComponent<Room>();
            if (lastRoom != null)
            {
                //get valid spawn point
                var validSpawners = lastRoom.roomSpawners.Where(x => !x.isOccupied).ToList();

                if (!validSpawners.Any())
                    Debug.Log("There are no valid placements for this tile, find another origin tile.");
                
                int rand = Random.Range(0, validSpawners.Count);
                Debug.Log("Choosing element " + rand + " of " + validSpawners.Count);
                var spawner = validSpawners[rand];
                tile.transform.position = spawner.gameObject.transform.position;

                //rotate next room until it fits AND when placed at least one spawn is valid
                //spawner.isOccupied = true;
                ////this is gross
                //var allTheSpawners = placedTiles.SelectMany(x => x.GetComponent<Room>().roomSpawners).ToList();
                //Debug.Log("Checking " + allTheSpawners.Count + " spawners for collision");
                //foreach (var sp in allTheSpawners)
                //{
                //    Collider[] hitColliders = Physics.OverlapSphere(sp.gameObject.transform.position, 0.1f);
                    
                //    if(hitColliders.Any())
                //        Debug.Log("Found " + hitColliders.Count() + " colliders");
                    
                //    foreach (var hitCollider in hitColliders)
                //    {
                //        Debug.Log("Hit " + hitCollider.gameObject.name);
                //        sp.isOccupied = true;
                //    }
                //}
            }
            lastRoom = currentRoom;
            deckIndex++;
        }
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
