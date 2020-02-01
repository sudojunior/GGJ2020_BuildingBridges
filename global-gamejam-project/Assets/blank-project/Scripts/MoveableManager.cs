using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableManager : MonoBehaviour
{
    // [SerializeField]
    // private GameObject player;
    [SerializeField]
    private GameObject slot;
    [SerializeField]
    private GameObject container;
    public GameObject holding;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Key down", gameObject);
            if(holding == null)
            {
                Pickup();
            }
            
            else if (!Physics.Raycast(transform.position, transform.forward, 2f))
            {
                Drop();
            }
        }
    }

    void Pickup()
    {
        RaycastHit hit;
        if(Physics.Raycast(gameObject.transform.position, transform.forward, out hit, 5f))
        {
            if (hit.collider.GetComponent<MoveableObject>() != null)
            {
                hit.collider.GetComponent<Rigidbody>().isKinematic = true;
                hit.transform.rotation = hit.collider.GetComponent<MoveableObject>().defaultRotation;

                hit.transform.position = slot.transform.position;
                hit.collider.transform.parent.SetParent(slot.transform);
                holding = hit.collider.gameObject;
            }
        }
    }

    void Drop()
    {
        holding.GetComponent<Rigidbody>().isKinematic = false;
        Debug.Log(transform.forward);
        holding.transform.parent.SetParent(container.transform);
        holding.transform.position = transform.position + (transform.forward * 2);
        holding = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward * 5f);
    }
}
