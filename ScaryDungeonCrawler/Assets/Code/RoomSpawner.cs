using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public RoomConnector connector;
    public bool isOccupied;

    void OnDrawGizmos()
    {
        Gizmos.color = isOccupied ? Color.red : Color.green;
        Gizmos.DrawSphere(transform.position, .5f);
    }
}
