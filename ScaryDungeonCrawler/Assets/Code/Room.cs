using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject[] walls;
    public RoomSpawner[] roomSpawners;
    public bool drawDebugRays = true;

    private void Update()
    {
        if(drawDebugRays)
        {
            foreach (RoomSpawner rs in roomSpawners)
            {
                Vector3 direction = (rs.gameObject.transform.position - transform.position).normalized * 2f;
                Vector3 origin = new Vector3(transform.position.x, 0.5f, transform.position.z);
                Debug.DrawRay(origin, direction, Color.magenta);
            }
        }
    }

    public bool IsValidPlacement(int index)
    {
        bool isValidPlacement = false;
        foreach (var rs in roomSpawners)
        {
            Vector3 direction = (rs.gameObject.transform.position - transform.position).normalized * 2f;
            Vector3 origin = new Vector3(transform.position.x, 0.5f, transform.position.z);
            var hits = Physics.RaycastAll(origin, direction, 2f);
            if (hits.Any())
                Debug.Log("hitInfo: " + string.Join(',', hits.Select(x => x.collider.gameObject.name)));

            isValidPlacement = hits.Any(x => x.collider.gameObject.name == "Outer");
            
            if(isValidPlacement)
                break;
        }
        return isValidPlacement;
    }

    public void RotateRoom()
    {
        Vector3 targetRotation = new Vector3();

        targetRotation += Vector3.up * 90f;
        if (targetRotation.y > 270f && targetRotation.y < 361f) targetRotation.y = 0f;
        if (targetRotation.y < 0f) targetRotation.y = 270f;
        transform.Rotate(targetRotation);
        foreach (var item in roomSpawners)
        {
            item.isOccupied = false;
        }
    }
}
