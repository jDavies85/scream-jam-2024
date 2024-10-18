using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public bool isOccupied;

    void OnDrawGizmos()
    {
        Gizmos.color = isOccupied ? Color.red : Color.green;
        Gizmos.DrawSphere(transform.position, .5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Floor"))
            isOccupied = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains("Floor"))
            isOccupied = false;
    }
}
