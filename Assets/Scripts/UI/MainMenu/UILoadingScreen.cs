using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UILoadingScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text loadingText;

    private static CanvasGroup cg;
    private static UILoadingScreen _instance;

    private void Awake()
    {
        _instance = this;
        cg = GetComponent<CanvasGroup>();

        cg.alpha = 0f;
        cg.blocksRaycasts = false;
        cg.interactable = false;
    }

    public static void LoadScene(string sceneName)
    {
        cg.alpha = 1f;
        cg.blocksRaycasts = true;
        cg.interactable = true;

        if (AudioController.Instance)
            AudioController.Instance.StopBGM();

        _instance.StartCoroutine(_instance.LoadLevelAsync(sceneName));
    }

    public static void ReloadScene()
    {
        if (SceneManager.GetActiveScene().name == "Level")
            EnemyManager.ClearEnemy();

        LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator LoadLevelAsync(string sceneName)
    {
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName);

        while (!loadOp.isDone)
        {
            int progressValue = (int)(Mathf.Clamp01(loadOp.progress / 0.9f) * 100);
            loadingText.text = "(" + progressValue.ToString() + "%)";
            yield return null;
        }
    }
}
