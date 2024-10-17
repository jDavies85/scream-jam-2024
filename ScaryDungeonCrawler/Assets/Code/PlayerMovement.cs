using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float rotateSpeed = 500f;
    public float collisionDetectionDistance = 10f;
    public int turns = 0;

    Vector3 targetPosition;
    Vector3 targetRotation;

    bool canMoveForward;
    bool canMoveBackward;
    RaycastHit forwardHit;
    RaycastHit backwardHit;

    bool IsMoving
    {
        get
        {
            return !((Vector3.Distance(transform.position, targetPosition) < 0.02f) &&
                    (Vector3.Distance(transform.eulerAngles, targetRotation) < 0.02f));
        }
    }

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward) * collisionDetectionDistance;
        Vector3 bwd = transform.TransformDirection(Vector3.back) * collisionDetectionDistance;
        Vector3 origin = new Vector3(transform.position.x, 0.5f, transform.position.z);
        Debug.DrawRay(origin, fwd, Color.green);
        Debug.DrawRay(origin, bwd, Color.green);
        canMoveForward = !Physics.Raycast(origin, fwd, out forwardHit, collisionDetectionDistance);
        canMoveBackward = !Physics.Raycast(origin, bwd, out backwardHit, collisionDetectionDistance);

        if(forwardHit.transform != null)
            Debug.Log("forwardHit: " + forwardHit.transform.gameObject.name);

        if (backwardHit.transform != null)
            Debug.Log("backwardHit: " + backwardHit.transform.gameObject.name);

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow)) MoveForward();
        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow)) MoveBackward();
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow)) RotateLeft();
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow)) RotateRight();
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

    public void RotateLeft() { if (!IsMoving) targetRotation -= Vector3.up * 90f; }
    public void RotateRight() { if (!IsMoving) targetRotation += Vector3.up * 90f; }
    public void MoveForward()
    {
        if (!IsMoving && canMoveForward)
        {
            targetPosition += transform.forward;
            turns++;
        }
    }
    public void MoveBackward()
    {
        if (!IsMoving && canMoveBackward)
        {
            targetPosition -= transform.forward;
            turns++;
        }
    }
}
