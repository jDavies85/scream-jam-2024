using UnityEngine;

public class RoomConnector : MonoBehaviour
{
    public Material onMaterial;
    public Material offMaterial;
    public bool isConnected = false;
    public Vector3 faceNormal;

    public void OnTileSet()
    {
        gameObject.GetComponent<Renderer>().material = onMaterial;
        isConnected = true;
    }

    public void OnTileUnSet()
    {
        gameObject.GetComponent<Renderer>().material = offMaterial;
        isConnected = false;
    }
}
