using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MoveableManager))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Physics")]
    private Rigidbody rigidBody;

    [Header("Position")]
    [SerializeField]
    private float movementSpeed;
    public float snapValue;
    private Vector3 nextPosition;
    private Vector3 direction;
    [SerializeField]
    private float ladderHeight;

    [SerializeField]
    private Vector3[] directionsToCheck;
    [SerializeField]
    private float checkDistance;
    [SerializeField]
    private float groundCheckDistance;

    [SerializeField]
    private float moveDelay;

    [SerializeField]
    private LayerMask maskCheck;
    [Space]

    [Header("Rotation")]
    [SerializeField]
    private float rotateSpeed;

    [Header("Input")]
    [SerializeField]
    private string moveXName;
    private float moveX;
    [SerializeField]
    private string moveZName;
    private float moveZ;

    [Header("Holding")]
    private MoveableManager moveableManager;

    [Header("Coroutines")]
    private Coroutine moveCoroutine;
    private Coroutine rotateCoroutine;

    public delegate void OnPlayerMove(Vector3 currentPosition);
    public event OnPlayerMove onPlayerMove;
    

    private void Start()
    {
        rigidBody = this.GetComponent<Rigidbody>();
        moveableManager = this.GetComponent<MoveableManager>();

        onPlayerMove += InvokePosition;
    }

    private void Update()
    {
        GetPlayerInput();
    }

    private void FixedUpdate()
    {
        if (moveX != 0 || moveZ != 0)
        {
            onPlayerMove?.Invoke(rigidBody.position);
        }
    }

    private void InvokePosition(Vector3 currentPosition)
    {
        if (rigidBody.velocity.y == 0)
        {
            rigidBody.isKinematic = true;

            if (moveX != 0)
            {
                nextPosition = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z + (snapValue * -moveX));
                direction = new Vector3(0, 0, -moveX);
            }

            else if (moveZ != 0)
            {
                nextPosition = new Vector3(currentPosition.x + (snapValue * moveZ), currentPosition.y, currentPosition.z);
                direction = new Vector3(moveZ, 0, 0);
            }

            nextPosition = RoundVector(nextPosition, snapValue);
            rotateCoroutine = StartCoroutine(AimPlayer(direction, rotateSpeed));

            RaycastHit hit;
            if (Physics.Raycast(rigidBody.position, direction, out hit, checkDistance, maskCheck))
            {
                //Ladder movement up
                if (hit.collider.GetComponent<Ladder>() != null && moveableManager.holding == null)
                {
                    nextPosition = hit.transform.position;
                    nextPosition.y = rigidBody.position.y + ladderHeight;

                    if (moveCoroutine == null) moveCoroutine = StartCoroutine(MovePosition(nextPosition, moveDelay));
                    rotateCoroutine = StartCoroutine(AimPlayer(direction, rotateSpeed));

                    return;
                }

                if (hit.collider.GetComponent<MoveableObject>() != null) return;

                else
                {
                    return;
                }
            }

            RaycastHit belowHit;
            if (Physics.Raycast(transform.position, -Vector3.up, out belowHit, 1f, maskCheck))
            {
                //Ladder movement down
                if (belowHit.collider.GetComponent<Ladder>() != null)
                {
                    if (moveCoroutine == null) moveCoroutine = StartCoroutine(MovePosition(nextPosition, moveDelay));
                    return;
                }
            }

            RaycastHit genericHit;
            //Regular movement
            if (Physics.Raycast(nextPosition, -Vector3.up, out genericHit, groundCheckDistance, maskCheck))
            {
                if (moveCoroutine == null) moveCoroutine = StartCoroutine(MovePosition(nextPosition, moveDelay));
                rotateCoroutine = StartCoroutine(AimPlayer(direction, rotateSpeed));

                if (Vector3.Distance(genericHit.point, rigidBody.position) >=  1.5f)
                {
                    rigidBody.isKinematic = false;
                }  

                return;
            }


            rigidBody.isKinematic = false;

            nextPosition.x += 1 * moveZ;
            nextPosition.z += 1 * moveX;

            if (Physics.Raycast(nextPosition, -Vector3.up, 1f))
            { 
                if (moveCoroutine == null) moveCoroutine = StartCoroutine(MovePosition(nextPosition, moveDelay));
                rotateCoroutine = StartCoroutine(AimPlayer(direction, rotateSpeed));
                return;
            }
        
        }
    }

    private IEnumerator MovePosition(Vector3 nextPosition, float moveDelay)
    {
        float t = 0;
        while (t <= .5f)
        {
            t += Time.deltaTime * movementSpeed;
            rigidBody.position = Vector3.Lerp(rigidBody.position, nextPosition, t);
            yield return new WaitForFixedUpdate();
        }

        rigidBody.position = nextPosition;

        yield return new WaitForSeconds(moveDelay);
        moveCoroutine = null;
    }

    private IEnumerator AimPlayer(Vector3 direction, float rotateSpeed)
    {
        float t = 0;
        while (t <= 1)
        {
            t += Time.deltaTime * rotateSpeed;
            rigidBody.transform.forward = Vector3.Slerp(rigidBody.transform.forward, direction, t);
            yield return new WaitForFixedUpdate();
        }
       
        rigidBody.transform.forward = direction;
        rotateCoroutine = null;
        yield return null;
    }

    private void GetPlayerInput()
    {
        moveX = Input.GetAxisRaw(moveXName);
        moveZ = Input.GetAxisRaw(moveZName);
    }

    public Vector3 RoundVector(Vector3 vector, float snap)
    {
        return new Vector3((vector.x / snap) * snap,
                           (vector.y / snap) * snap,
                           (vector.z / snap) * snap);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * checkDistance);
        Gizmos.DrawRay(nextPosition, -Vector3.up * groundCheckDistance);
    }

}
