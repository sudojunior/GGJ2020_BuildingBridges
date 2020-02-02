using UnityEngine;

public class ObjectRespawner : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<MoveableObject>() != null)
        {
            MoveableObject hitObject = col.GetComponent<MoveableObject>();
            hitObject.ResetState();
            hitObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
