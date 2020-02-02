using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class ObjectiveItem
{
    public Transform finalPosition;
    public GameObject placedObject;
}

public class Objective : MonoBehaviour
{
    public ObjectiveItem[] objectives;
    public float animationSpeed;

    private void OnTriggerEnter(Collider col)
    {
        ObjectiveItem item = IsItemAnObjective(col.transform.gameObject);
        if (item != null && MoveableManager.Instance.holding == null)
        {
            MoveableObject moveableObject = col.GetComponent<MoveableObject>();
            col.transform.rotation = moveableObject.defaultRotation;
            Destroy(moveableObject);

            Destroy(col.GetComponent<Collider>());
            col.gameObject.layer = 8;
            Destroy(col.GetComponent<Rigidbody>());
            StartCoroutine(MoveObject(item, animationSpeed));
        }

    }

    private IEnumerator MoveObject(ObjectiveItem objectiveItem, float speed)
    { 
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * speed;
            objectiveItem.placedObject.transform.position = Vector3.Lerp(objectiveItem.placedObject.transform.position, objectiveItem.finalPosition.position, t);
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }

    private ObjectiveItem IsItemAnObjective(GameObject gameObject)
    {
        for (int i = 0; i < objectives.Length; i++)
        {  
            if (gameObject == objectives[i].placedObject)
            {
                return objectives[i];
            }
        }
        return null;
    }
}
