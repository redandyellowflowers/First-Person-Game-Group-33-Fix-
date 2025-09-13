using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerScript : MonoBehaviour
{
    /*
    1.
    Title: How to make a LOADING BAR in Unity
    Author: Asbjørn Thirslund / Brackeys
    Date: 16 August 2025
    Code version: 1
    Availability: https://www.youtube.com/watch?v=YMj2qPq9CP8&t=258s

    2.
    Title: "Sophies Heroic Bloodbath - Assignment 4 - Exam - Playable Game"
    Author: Miguel Marindanise, Fatima Zahraa Bham, Yongama Ntloko 
    Date: 16 August 2025
    Code version: 1
    */

    public static SceneManagerScript sceneManager;

    [Header("loading screen ui")]
    public GameObject loadingScreen;
    public TextMeshProUGUI progressPercentageText;
    public Slider slider;

    void Awake()
    {
        Time.timeScale = 1f;
    }

    private void Start()
    {

    }

    public void NextLevel()
    {
        StartCoroutine(LoadNextLevelAsynchronously());
    }

    IEnumerator LoadNextLevelAsynchronously()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        loadingScreen.SetActive(true);

        while (operation.isDone == false)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = progress;

            if (progressPercentageText != null)
            {
                progressPercentageText.text = progress * 100f + "%";
            }

            yield return null;
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsynchronously(sceneName));
    }

    IEnumerator LoadSceneAsynchronously(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        loadingScreen.SetActive(true);

        while (operation.isDone == false)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = progress;

            if (progressPercentageText != null)
            {
                progressPercentageText.text = progress * 100f + "%";
            }

            yield return null;
        }
    }

    public void Restart()
    {
        StartCoroutine(LoadActiveSceneAsynchronously());
    }

    IEnumerator LoadActiveSceneAsynchronously()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);

        loadingScreen.SetActive(true);

        while (operation.isDone == false)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = progress;

            if (progressPercentageText != null)
            {
                progressPercentageText.text = progress * 100f + "%";
            }

            yield return null;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit.");
    }
}
