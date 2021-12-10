// Author: Fatima Nadeem / Croft

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{

    public static GameSceneManager instance;

    public GameObject mainMenuInstruction; // Main menu click instruction object
    public Scene[] scenes;
    public float timeTillDialogue = 1.34f; // Must be greater than fade-in/
                                           // fade-out time of a scene - see
                                           // animator (40/60 + 40/60) seconds

    Queue<Scene> loadedScenes;
    Scene currentScene;
    string currentMusic;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        mainMenuInstruction.SetActive(false);
        
        loadedScenes = new Queue<Scene>();

        currentMusic = "";

        StartGame();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AudioManager.instance.Play("Click");

            if (currentScene.isMainMenuInstead)
            {
                mainMenuInstruction.SetActive(false);
                EndCurrentScene();
            }
        }
    }

    void StartGame()
    {
        // Loading all scenes
        foreach (Scene scene in scenes)
        {
            loadedScenes.Enqueue(scene);
        }

        StartNewScene();
    }

    void StartNewScene()
    {
        if (loadedScenes.Count > 0)
        {
            currentScene = loadedScenes.Dequeue();

            currentScene.gameObject.SetActive(true); // Activating current
                                                     // scene's game object

            if (currentScene.musicName != currentMusic)
            // if current scene's background music is not the same as previous
            // scene's background music
            {
                // Stopping previous scene's background music and playing
                // current scene's background music
                AudioManager.instance.Stop(currentMusic);
                currentMusic = currentScene.musicName;
                AudioManager.instance.Play(currentMusic);
            }

            StartCoroutine(SceneStartEnd(true));
        }
        else
        {
            StartGame(); // Restart game
        }
    }

    public void EndCurrentScene()
    {
        StartCoroutine(SceneStartEnd(false));
    }

    IEnumerator SceneStartEnd(bool start = true)
    /* This function takes care of any time delays in the start and the end of 
     * the current scene.
     */
    {
        if (start)
        {
            yield return new WaitForSeconds(timeTillDialogue);
            // Waiting for scene fade-in animation

            if (currentScene.isMainMenuInstead)
            {
                mainMenuInstruction.SetActive(true);
            }
            else
            {
                DialogueManager.instance.StartDialogues(currentScene);
                //Asking dialogue manager to start current scene's dialogues
            }
        }
        else
        {
            // Playing scene fade-out animation and waiting for it
            currentScene.GetComponent<Animator>().SetTrigger("FadeOut");
            yield return new WaitForSeconds(timeTillDialogue);

            currentScene.gameObject.SetActive(false); // Deactivating current
                                                      // scene's game object 

            StartNewScene();
        }
    }
}
