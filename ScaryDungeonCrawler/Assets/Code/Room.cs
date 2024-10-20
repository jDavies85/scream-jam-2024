using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Room : MonoBehaviour
{
    public GameObject[] walls;
    public List<GameObject> neighbours = new List<GameObject>();
    public RoomSpawner[] roomSpawners;
    public bool drawDebugRays = true;

    private List<GameObject> checkedNeighbours = new List<GameObject>();

    //private void Update()
    //{
    //    if(drawDebugRays)
    //    {
    //        foreach (RoomSpawner rs in roomSpawners)
    //        {
    //            Vector3 direction = (rs.gameObject.transform.position - transform.position).normalized * 2f;
    //            Vector3 origin = new Vector3(transform.position.x, 0.5f, transform.position.z);
    //            Debug.DrawRay(origin, direction, Color.magenta);
    //        }
    //    }
    //}

    public bool IsValidPlacementV2(Room room, RoomConnector origin)
    {
        var roomConnectors = room.roomSpawners.Select(x => x.connector).ToList();
        foreach (var item in roomConnectors)
        {
            if (AreFacesFacingEachOther(item, origin))
            {
                item.OnTileSet();
                origin.OnTileSet();
            }
            else
            {
                item.OnTileUnSet();
                origin.OnTileUnSet();
            }
        }

        return roomConnectors.Any(x => x.isConnected);
    }

    public void CheckNeighbours()
    {
        foreach(var item in roomSpawners.Select(x => x.connector))
        {
            foreach (var neighbour in neighbours)
            {
                foreach (var roomConnector in neighbour.GetComponent<Room>().roomSpawners.Select(x => x.connector).Where(x => !x.isConnected))
                {
                    if (AreFacesFacingEachOther(item, roomConnector))
                    {
                        item.OnTileSet();
                        roomConnector.OnTileSet();
                    }
                }
            }
        }
    }

    bool AreFacesFacingEachOther(RoomConnector rca, RoomConnector rcb)
    {
        // Transform the local face normals to world space
        Vector3 worldNormal1 = rca.transform.TransformDirection(rca.faceNormal);
        Vector3 worldNormal2 = rcb.transform.TransformDirection(rcb.faceNormal);

        // Check if the normals are opposite (dot product close to -1)
        float dotProduct = Vector3.Dot(worldNormal1, worldNormal2);
        return dotProduct < -0.99f; // Threshold for considering them as facing each other
    }

    public void RotateRoom()
    {
        Vector3 targetRotation = new Vector3();

        targetRotation += Vector3.up * 90f;
        if (targetRotation.y > 270f && targetRotation.y < 361f) targetRotation.y = 0f;
        if (targetRotation.y < 0f) targetRotation.y = 270f;
        transform.Rotate(targetRotation);
    }
}
