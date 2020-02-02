using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class MoveableManager : MonoBehaviour
{
    // [SerializeField]
    // private GameObject player;
    [SerializeField]
    private GameObject slot;
    [SerializeField]
    private GameObject container;
    public GameObject holding;

    [SerializeField]
    private float rotationSnap = 90.0f;

    private Coroutine moveCoroutine;
    private Coroutine rotateCoroutine;

    private PlayerMovement playerMovement;

    public static MoveableManager Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = this.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        // Space - Pickup / Drop
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(holding == null)
            {
                Pickup();
            }
            
            else if (!Physics.Raycast(transform.position, transform.forward, 2f))
            {
                Drop();
            }
            return;
        }

        // Rotation
        // Q - Left
        // E - Right
        if(holding != null)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                RotateObject(-rotationSnap);
            }

            if(Input.GetKeyDown(KeyCode.E))
            {
                RotateObject(rotationSnap);
            }
        }
    }

    void Pickup()
    {
        RaycastHit hit;
        if(Physics.Raycast(gameObject.transform.position, transform.forward, out hit, 2f))
        {
            if (hit.collider.GetComponent<MoveableObject>() != null)
            {
                hit.collider.GetComponent<Rigidbody>().isKinematic = true;
                MoveableObject MO_Component = hit.collider.GetComponent<MoveableObject>();
                moveCoroutine = StartCoroutine(LerpPosition(hit.collider.gameObject, slot.transform.position, 5f));

                // hit.transform.position = slot.transform.position;
                hit.collider.transform.SetParent(slot.transform);
                holding = hit.collider.gameObject;
            }
        }
    }

    void Drop()
    {
        holding.GetComponent<Rigidbody>().isKinematic = false;

        moveCoroutine = StartCoroutine(LerpPosition(holding, transform.position + (transform.forward * 2), 5f));

        holding.transform.SetParent(container.transform);

        holding = null;
    }

    private IEnumerator LerpPosition(GameObject target, Vector3 destination, float moveSpeed)
    {
        float t = 0;
        while (t <= 1)
        {
            t += Time.deltaTime * moveSpeed;
            target.transform.position = Vector3.Lerp(target.transform.position, destination, t);
            yield return new WaitForFixedUpdate();
        }
        target.transform.position = playerMovement.RoundVector(destination, playerMovement.snapValue);
        yield return null;
        moveCoroutine = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward * 5f);
    }

    private void RotateObject(float rotateAmount)
    {
        if (rotateCoroutine == null)
            rotateCoroutine = StartCoroutine(LerpRotateObject(rotateAmount, 5f));
    }

    // Y axis only
    private IEnumerator LerpRotateObject(float rotateAmount, float rotateSpeed)
    {
        float t = 0;
        Vector3 eulerAngles = holding.transform.eulerAngles;
        Vector3 target = new Vector3(
            eulerAngles.x,
            eulerAngles.y + rotateAmount,
            eulerAngles.z
        );

        while (t <= 1)
        {
            t += Time.deltaTime * rotateSpeed;
            holding.transform.eulerAngles = Vector3.Lerp(eulerAngles, target, t);
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(0.1f);
        rotateCoroutine = null;
    }

    private IEnumerator LerpRotateObject(GameObject target, Vector3 eulerAngles, float rotateSpeed)
    {
        float t = 0;
        while(t <= 1)
        {
            t += Time.deltaTime * rotateSpeed;
            holding.transform.eulerAngles = Vector3.Slerp(target.transform.eulerAngles, eulerAngles, t);
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(0.1f);
        rotateCoroutine = null;
    }
}
