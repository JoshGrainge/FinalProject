using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField]
    Button playButton, quitButton;

    [SerializeField]
    GameObject loadingBarParent, loadingBarOrigin;

    void Start()
    {
        playButton.onClick.AddListener(Play);
        quitButton.onClick.AddListener(Quit);

    }

    /// <summary>
    /// Load world scene when play button is pressed
    /// </summary>
    void Play()
    {
        // Disable buttons
        playButton.enabled = false;
        quitButton.enabled = false;

        StartCoroutine(StartLoading());
    }

    /// <summary>
    /// Load scene asynchronously and update loading bar progress
    /// </summary>
    /// <returns></returns>
    IEnumerator StartLoading()
    {
        loadingBarOrigin.SetActive(true);

        int loadingScreenIndex = 1;
        AsyncOperation loadingProgress = SceneManager.LoadSceneAsync(loadingScreenIndex);

        // Update progress bar percentage
        while(!loadingProgress.isDone)
        {
            float progress = loadingProgress.progress;
            Vector3 newLocalScale = loadingBarOrigin.transform.localScale;
            newLocalScale.x = progress;
            loadingBarOrigin.transform.localScale = newLocalScale;

            yield return null;
        }

        loadingProgress.allowSceneActivation = true;
    }


    /// <summary>
    /// Quits applicaiton on button press
    /// </summary>
    void Quit()
    {
        Application.Quit();
    }
}
