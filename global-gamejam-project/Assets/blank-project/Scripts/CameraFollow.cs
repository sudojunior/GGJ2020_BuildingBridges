using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    private Vector3 offset;
    [SerializeField]
    private float speed;

    private void Start()
    {
        offset = this.transform.position - target.position;
    }

    private void Update()
    {
        
        FollowTarget(target.position, offset, speed);
    }

    private void FollowTarget(Vector3 targetPosition, Vector3 offset, float speed)
    {
        Vector3 newPosition = targetPosition + offset;
        this.transform.position = Vector3.Lerp(this.transform.position, newPosition, Time.deltaTime * speed);
    }
}
