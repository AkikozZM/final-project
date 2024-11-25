using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GUIController : MonoBehaviour
{   
    public GameSettings gameSettings;
    public Button restartButton;
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI starCountText;
    public TextMeshProUGUI targetCountText;
    public float fadeDuration = 1f;

    private int currentStarCount = 0;
    private int currentTargetCount = 0;
    void Start()
    {
        currentStarCount = 0;
        currentTargetCount = gameSettings.maxObjects;
        starCountText.text = currentStarCount.ToString();
        targetCountText.text = currentTargetCount.ToString();
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(StartFadeOutAndRestart);
        }
    }
    public void updateTargetCount(int n)
    {
        currentTargetCount = n;
        targetCountText.text = currentTargetCount.ToString();
    }
    public void updateStarCount(int n)
    {
        currentStarCount = n;
        starCountText.text = currentStarCount.ToString();
    }

    public void StartFadeOutAndRestart()
    {
        StartCoroutine(FadeOutAndRestart());
    }
    IEnumerator FadeOutAndRestart()
    {
        // Fade out the UI
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = 1f - Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
