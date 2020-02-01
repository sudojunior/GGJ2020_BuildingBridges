using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    void OnTriggerEnter(Collider other)
    {
        TriggerDialogue();
        Destroy(this.gameObject);
    }

    public void TriggerDialogue()
    {   
        Debug.Log("Dialogue has started");
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
