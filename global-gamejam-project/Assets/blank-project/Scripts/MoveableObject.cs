using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    public Vector3 defaultPosition;
    public Quaternion defaultRotation;

    public virtual void Start()
    {
        defaultPosition = this.transform.position;
        defaultRotation = this.transform.rotation;
    }

    public void ResetState()
    {
        this.transform.position = defaultPosition;
        this.transform.rotation = defaultRotation;
    }
}
