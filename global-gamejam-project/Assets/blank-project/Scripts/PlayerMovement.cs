using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Physics")]
    private Rigidbody rigidBody;

    [Header("Position")]
    [SerializeField]
    private float snapValue;
    private Vector3 nextPosition;

    [Header("Input")]
    [SerializeField]
    private string moveXName;
    private float moveX;
    [SerializeField]
    private string moveZName;
    private float moveZ;

    private void Start()
    {
        rigidBody = this.GetComponent<Rigidbody>();
    }

    private void GetPlayerInput()
    {
        moveX = Input.GetAxis(moveXName);
        moveZ = Input.GetAxis(moveZName);
    }

    private Vector3 RoundVector(Vector3 vector, float snap)
    {
        return new Vector3((vector.x / snap) * snap,
                           (vector.y / snap) * snap,
                           (vector.z / snap) * snap);
    }
}
