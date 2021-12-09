// Author: Fatima Nadeem / Croft

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
/*  This class is the central dialogue managment system. It runs any dialogues 
 *  passed to it on any dialogue box of choice.
 */

{
    public bool dialogueRunning;
    public Text dialogueText;
    
    private Animator dialogueBoxAnimator;
    private Queue<string> sentences; // A queue that holds a dialogue's
                                     // individual sentences
    private Coroutine currentTypeSentence; // the current sentence typing
                                           // coroutine

    void Start()
    {
        dialogueBoxAnimator = GetComponent<Animator>();
        sentences = new Queue<string>();
        dialogueRunning = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0)) // if left mouse button is pressed
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialogueRunning = true;
        dialogueBoxAnimator.SetBool("isOpen", true); // running fading in
                                                     // animation for dialogue
                                                     // box and text

        sentences.Clear(); // emptying queue of any sentences from the previous
                           // dialogue

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    private void DisplayNextSentence()
    /*  This function displays the next sentence in the dialogue (or the next 
     *  sentence to dequeue in sentences queue).
     */
    {
        if (sentences.Count == 0) // if no sentences remain to display
        {
            EndDialogue();
            return;
        }

        if (currentTypeSentence != null) // if any previous sentence is running
        {
            StopCoroutine(currentTypeSentence); // stopping the typing
        }
        currentTypeSentence = StartCoroutine(TypeSentence(sentences.Dequeue()));
        // typing next sentence
    }

    IEnumerator TypeSentence(string sentence)
    /*  This function displays a sentence with a typing animation.
     */
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null; // waiting a single frame
        }
    }

    private void EndDialogue()
    {
        dialogueBoxAnimator.SetBool("isOpen", false); // fading out dialogue
                                                      // box and text
        dialogueRunning = false;
    }

    public void HaltDialogue()
    {
        sentences.Clear(); // emptying queue of any sentences from the previous
                           // dialogue

        StopCoroutine(currentTypeSentence); // stopping the typing

        EndDialogue();
    }
}
