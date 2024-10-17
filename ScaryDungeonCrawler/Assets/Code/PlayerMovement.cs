using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float rotateSpeed = 500f;
    public float collisionDetectionDistance = 10f;
    public int turns = 0;

    Vector3 targetPosition;
    Vector3 targetRotation;

    bool IsMoving
    {
        get
        {
            return !((Vector3.Distance(transform.position, targetPosition) < 0.02f) &&
                    (Vector3.Distance(transform.eulerAngles, targetRotation) < 0.02f));
        }
    }

    public void RotateLeft() { if (!IsMoving) targetRotation -= Vector3.up * 90f; }
    public void RotateRight() { if (!IsMoving) targetRotation += Vector3.up * 90f; }
    public void MoveForward()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward) * collisionDetectionDistance;

        if (!Physics.Raycast(transform.position, fwd, out RaycastHit hitinfo, collisionDetectionDistance))
        {
            Debug.Log("Nothing in front of you");
            if (!IsMoving)
            {
                Debug.Log("IsMoving: " + IsMoving);
                targetPosition += transform.forward;
                turns++;
            }
        }
    }

    public void MoveBackward()
    {
        Vector3 bwd = transform.TransformDirection(Vector3.back) * collisionDetectionDistance;

        if (!Physics.Raycast(transform.position, bwd, collisionDetectionDistance))
        {
            if (!IsMoving)
            {
                targetPosition -= transform.forward;
                turns++;
            }
        }
    }

    
    void Start()
    {
        targetPosition = transform.position;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector3 target = targetPosition;

        if (targetRotation.y > 270f && targetRotation.y < 361f) targetRotation.y = 0f;
        if (targetRotation.y < 0f) targetRotation.y = 270f;

        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * moveSpeed);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * rotateSpeed);
    }

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward) * collisionDetectionDistance;
        Debug.DrawRay(transform.position, forward, Color.green);

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow)) MoveForward();
        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow)) MoveBackward();
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow)) RotateLeft();
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow)) RotateRight();
    }
}
