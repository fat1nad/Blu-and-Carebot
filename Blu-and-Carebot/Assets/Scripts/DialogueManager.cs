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
    public SceneManager sM;
    public Image characterImage;
    public Image backgroundImage;
    public Text dialogueText;
    public Animator dialogueBoxAnimator;
    public GameObject instruction;

    bool dialoguesRunning;
    string characterName;
    Queue<Dialogue> dialogues; // A queue that holds all dialogues of a
                                       // scene
    Queue<string> sentences; // A queue that holds a dialogue's
                                     // individual sentences
    Coroutine currentTypeSentence; // the current sentence typing
                                           // coroutine

    void Start()
    {
        dialogues = new Queue<Dialogue>();
        sentences = new Queue<string>();
        dialoguesRunning = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && dialoguesRunning) 
            // if left mouse button is pressed
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogues(Scene scene)
    {
        dialoguesRunning = true;
        dialogueBoxAnimator.SetTrigger("Open"); // running fading-in animation
                                                // for dialogue box and text

        dialogues.Clear(); // emptying queue of any dialogues from the previous
                           // scene

        foreach (Dialogue dialogue in scene.dialogues)
        {
            dialogues.Enqueue(dialogue);
        }

        RunNextDialogue();
    }


    void StartDialogue(Dialogue dialogue)
    {
        sentences.Clear(); // emptying queue of any sentences from the previous
                           // dialogue

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        characterName = dialogue.characterName;
        characterImage.sprite = dialogue.characterImage;

        DisplayNextSentence();
    }

    void RunNextDialogue()
    {
        if (dialogues.Count == 0) // if no dialogues remain to run
        {
            EndDialogues();
            return;
        }

        StartDialogue(dialogues.Dequeue());
    }

    void EndDialogues()
    {
        dialoguesRunning = false;

        dialogueBoxAnimator.SetTrigger("Close"); // running fading-in animation
                                                 // for dialogue box and text
        instruction.SetActive(false);
        sM.EndCurrentScene();
    }

    void DisplayNextSentence()
    /*  This function displays the next sentence in the dialogue (or the next 
     *  sentence to dequeue in sentences queue).
     */
    {
        if (sentences.Count == 0) // if no sentences remain to display
        {
            RunNextDialogue();
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
        instruction.SetActive(false);

        dialogueText.text = characterName + ": ";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null; // waiting a single frame
        }

        instruction.SetActive(true);
    }
}
