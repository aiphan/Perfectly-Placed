using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public GameObject[] tools;
    private bool levelCompleted;
    public string nextSceneName = "PauseMenu";

    private void Awake() => Instance = this;

    private void Start() => tools = tools.Length == 0 ? FindTools("Tools") : tools;

    public void CheckForCompletion()
    {
        if (!levelCompleted && AreAllToolsCorrect()) LevelComplete();
    }

    private bool AreAllToolsCorrect()
    {
        foreach (var tool in tools)
            if (!tool.GetComponent<ToolSnap>().IsCorrectlyPlaced) return false;
        return true;
    }

    private void LevelComplete()
    {
        levelCompleted = true;
        AudioManager.Instance.PlayLevelCompleteSound();
        AudioManager.Instance.FadeOutBackgroundMusic(2f);
        StartCoroutine(TransitionAfterDelay(2f));
    }

    private IEnumerator TransitionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(nextSceneName);
    }

    private GameObject[] FindTools(string layer)
    {
        var allObjects = GameObject.FindObjectsOfType<GameObject>();
        var toolsList = new List<GameObject>();
        foreach (var obj in allObjects)
        {
            var sr = obj.GetComponent<SpriteRenderer>();
            if (sr != null && sr.sortingLayerName == layer) toolsList.Add(obj);
        }
        return toolsList.ToArray();
    }
}
