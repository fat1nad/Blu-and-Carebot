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
    public static DialogueManager instance;

    public Image characterImage;
    public Text dialogueText;
    public GameObject instruction;
    public float timeBetweenLetters = 0.03f;
    public float dialogueBoxFadeInTime = 0.67f; // 40/60 second

    Animator dialogueSysAnimator;
    bool dialoguesRunning;
    string characterName;
    Queue<Dialogue> dialogues; // A queue that holds all dialogues of a scene
    Queue<string> sentences; // A queue that holds a dialogue's individual
                             // sentences
    Coroutine currentTypeSentence; // the current sentence typing coroutine

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        dialogueSysAnimator = GetComponent<Animator>();
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
    /*  This function starts dialogues in a scene.
     */
    {
        dialogues.Clear(); // emptying queue of any dialogues from the previous
                           // scene

        dialoguesRunning = true;
        dialogueSysAnimator.SetTrigger("Open"); // running fade-in animation
                                                // for dialogue box and text

        StartCoroutine(TypeSentence("", false)); // Waiting for the animation
                                                 // before starting dialogues

        // Loading dialogues into dialogues queue
        foreach (Dialogue dialogue in scene.dialogues)
        {
            dialogues.Enqueue(dialogue);
        }

        RunNextDialogue();
    }

    void RunNextDialogue()
    /*  This function runs the next dialogue in the scene (or the next dialogue
     *  to dequeue in dialogues queue).
     */
    {
        if (dialogues.Count == 0) // if no dialogues remain in the queue
        {
            EndDialogues();
            return;
        }

        StartDialogue(dialogues.Dequeue());
    }

    void EndDialogues()
    /*  This function ends dialogues in the current scene.
     */
    {
        dialoguesRunning = false;

        dialogueSysAnimator.SetTrigger("Close"); // running fade-out animation
                                                 // for dialogue box and text
        instruction.SetActive(false); // Hiding "Click to proceed" instruction
        
        GameSceneManager.instance.EndCurrentScene(); // Asking scene manager
                                                     // to end current scene
    }

    void StartDialogue(Dialogue dialogue)
    {
        sentences.Clear(); // emptying queue of any sentences from the previous
                           // dialogue

        // Loading sentences into sentences queue
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        characterName = dialogue.characterName;
        characterImage.sprite = dialogue.characterImage; // Must be 340x340

        DisplayNextSentence();
    }

    void DisplayNextSentence()
    /*  This function displays the next sentence in the dialogue (or the next 
     *  sentence to dequeue in sentences queue).
     */
    {
        instruction.SetActive(false);

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

    IEnumerator TypeSentence(string sentence, bool typing = true)
    /*  This function displays a sentence with typing animation if typing is 
     *  true, else it waits for the dialogue box fade-in animation.
     */
    {
        if (typing)
        {
            dialogueText.text = characterName + ": ";
            foreach (char letter in sentence.ToCharArray())
            {
                AudioManager.instance.Play("Letter Sound");
                dialogueText.text += letter;
                yield return new WaitForSeconds(timeBetweenLetters);
            }

            instruction.SetActive(true);
        }
        else
        {
            yield return new WaitForSeconds(dialogueBoxFadeInTime);
        }       
    }
}
