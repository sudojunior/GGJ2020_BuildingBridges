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
    private GameObject holding;

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
            } else
            {
                Drop();
            }
        }
    }

    void Pickup()
    {
        RaycastHit hit;
        if(Physics.Raycast(gameObject.transform.position, Vector3.forward, out hit, 5f))
        {
            if (hit.collider.GetComponent<MoveableObject>() != null)
            {
                Debug.Log("Raycast", hit.collider.gameObject);
                hit.collider.transform.SetParent(slot.transform);
                holding = hit.collider.gameObject;
            }
        }
    }

    void Drop()
    {
        holding.transform.position = gameObject.transform.forward;
        holding.transform.SetParent(container.transform);
        holding = null;
    }
}
