using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ladder : MoveableObject
{

    private Rigidbody rigidBody;

    public override void Start()
    {
        base.Start();
        rigidBody = this.GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.transform.position.y < rigidBody.position.y) rigidBody.isKinematic = true;
    }
}
