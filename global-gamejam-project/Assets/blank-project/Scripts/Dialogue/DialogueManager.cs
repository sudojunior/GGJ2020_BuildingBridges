using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public Animator animator;

    private Queue<Sentences> sentences;


    void Start()
    {
        sentences = new Queue<Sentences>();
    }

    public void StartDialogue (Dialogue dialogue)
    {
        Debug.Log("Starting conversation with " + dialogue.sentences[0].name);

        animator.SetBool("IsOpen", true);

        nameText.text = dialogue.title;

        sentences.Clear();

        foreach (Sentences sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        Sentences sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));

    }

    IEnumerator TypeSentence (Sentences sentence)
    {
        dialogueText.text = "";
        nameText.text = sentence.name;
        foreach (char letter in sentence.sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        Debug.Log("End of conversation.");
        animator.SetBool("IsOpen", false);
    }
}
