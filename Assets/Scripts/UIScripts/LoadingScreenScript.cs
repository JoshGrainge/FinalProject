using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenScript : MonoBehaviour
{
    [SerializeField]
    GameObject loadingBarScaler;

    void Awake()
    {
        StartCoroutine(StartLoadLevel());
    }

    IEnumerator StartLoadLevel()
    {
        int worldSceneIndex = 2;
        AsyncOperation loadingProgress = SceneManager.LoadSceneAsync(worldSceneIndex);

        // Update progress bar size while scene is still loading
        while (!loadingProgress.isDone)
        {
            float progress = loadingProgress.progress;
            Vector3 newLocalScale = loadingBarScaler.transform.localScale;
            newLocalScale.x = progress;
            loadingBarScaler.transform.localScale = newLocalScale;

            yield return null;
        }

    }

}
