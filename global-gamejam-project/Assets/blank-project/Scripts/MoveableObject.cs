using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    public Vector3 defaultPosition;
    public Quaternion defaultRotation;

    private void Start()
    {
        defaultPosition = this.transform.position;
        defaultRotation = this.transform.rotation;
    }
}
