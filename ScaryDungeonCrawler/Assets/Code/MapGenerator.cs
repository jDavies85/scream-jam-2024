using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class MapGenerator : MonoBehaviour
{
    public DungeonTile[] tiles = new DungeonTile[6];
    public bool generateRandomMap;
    private List<GameObject> deck = new List<GameObject>();

    void Awake()
    {
        if(generateRandomMap)
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < tiles[i].count; j++)
                {
                    deck.Add(Instantiate(tiles[i].prefab));
                }
            }
            deck.Shuffle();
            var coords = new Vector3();
            int rowCount = 4;
            for (int i = 0; i < deck.Count; i++)
            {
                deck[i].transform.position = coords;
                coords.x += 3;
                rowCount--;
                if(rowCount == 0)
                {
                    coords.x = 0;
                    coords.z += 3;
                    rowCount = 4;
                }
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
