using System.Linq;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public Room room;
    public RoomConnector roomConnector;

    //private void Update()
    //{
    //    var roomConncters = room.roomConnectors.Any(x => AreFacesFacingEachOther(x, roomConnector));
    //    foreach (var item in room.roomConnectors)
    //    {
    //        if (AreFacesFacingEachOther(item, roomConnector))
    //        {
    //           item.OnTileSet();
    //           roomConnector.OnTileSet();
    //        }
    //        else
    //        {
    //            item.OnTileUnSet();
    //            roomConnector.OnTileUnSet();
    //        }
    //    }
    //}

    bool AreFacesFacingEachOther(RoomConnector rca, RoomConnector rcb)
    {
        // Transform the local face normals to world space
        Vector3 worldNormal1 = rca.transform.TransformDirection(rca.faceNormal);
        Vector3 worldNormal2 = rcb.transform.TransformDirection(rcb.faceNormal);

        // Check if the normals are opposite (dot product close to -1)
        float dotProduct = Vector3.Dot(worldNormal1, worldNormal2);
        return dotProduct < -0.99f; // Threshold for considering them as facing each other
    }

}
