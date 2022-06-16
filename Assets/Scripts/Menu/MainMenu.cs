using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Загрузка игры")]
    AsyncOperation asyncLoading;
    public TextMeshProUGUI loadingText;
    //public int ScenID;

    public void PlayGame()
    {
        StartCoroutine(LoadSceneCor());
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadSceneCor()
    {
        yield return new WaitForSeconds(1f);
        asyncLoading = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        while (!asyncLoading.isDone)
        {
            float progress = asyncLoading.progress / 0.9f;
            loadingText.enabled = true;
            loadingText.text = "Loading...  " + string.Format("{0:0}%", progress * 100f);
            yield return 0;
        }
    }
}
