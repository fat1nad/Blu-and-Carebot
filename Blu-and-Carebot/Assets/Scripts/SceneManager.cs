using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public DialogueManager dM;
    public Scene[] scenes;
    public int dialoguePause = 10;

    Scene currentScene;
    Queue<Scene> loadedScenes;

    void Start()
    {
        loadedScenes = new Queue<Scene>();
        
        foreach(Scene scene in scenes)
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
            currentScene.gameObject.SetActive(true);
            StartCoroutine(SceneStartEnd(true));
        }
    }
    public void EndCurrentScene()
    {
        StartCoroutine(SceneStartEnd(false));
    }

    IEnumerator SceneStartEnd(bool start = true)
    {

        Animator currentSceneBOAnim = currentScene.sceneBlackOutAnim;

        if (start)
        {
            currentSceneBOAnim.SetTrigger("FadeIn");

            yield return new WaitForSeconds(dialoguePause); // hard code : (

            dM.StartDialogues(currentScene);
        }
        else
        {
            currentSceneBOAnim.SetTrigger("FadeOut");

            yield return new WaitForSeconds(dialoguePause); // hard code : (

            StartNewScene();
        }
    }
}
