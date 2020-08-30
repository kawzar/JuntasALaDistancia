using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField]
    private CanvasGroup gameOverCanvas;

    [SerializeField]
    private CanvasGroup mainMenuCanvas;

    [SerializeField]
    private CanvasGroup countdownCanvas;

    [SerializeField]
    private TextMeshProUGUI countdownText;

    [SerializeField]
    private int countdownFrom = 3;

    [SerializeField]
    private float fadeDuration;

    [SerializeField]
    public int menuSceneBuildNumber = 0;

    [SerializeField]
    public int creditsSceneBuildNumber = 2;

    [SerializeField]
    public int gameplaySceneBuildNumber = 1;

    private void Awake()
    {
        Instance = this;
    }

    public void DisplayGameOverScreen(bool display)
    {
        if (display)
        {
            gameOverCanvas.gameObject.SetActive(true);
            gameOverCanvas.DOFade(0.9f, fadeDuration);
        }
        else
        {
            gameOverCanvas.DOFade(0, fadeDuration).OnComplete(() => gameOverCanvas.gameObject.SetActive(false));
        }
    }

    public void OnMenuButtonClicked()
    {
        if (gameOverCanvas.isActiveAndEnabled)
        {
            gameOverCanvas.DOFade(0, fadeDuration).OnComplete(() =>
            {
                gameOverCanvas.gameObject.SetActive(false);
                SceneManager.LoadScene(menuSceneBuildNumber);
            });
        }
        else
        {
            SceneManager.LoadScene(menuSceneBuildNumber);
        }
    }

    public void OnRestartButtonClicked()
    {
        gameOverCanvas.DOFade(0, fadeDuration).OnComplete(() =>
        {
            gameOverCanvas.gameObject.SetActive(false);
            SceneManager.LoadScene(gameplaySceneBuildNumber);
        });
    }

    public void OnPlayButtonClicked()
    {
        mainMenuCanvas.DOFade(0, fadeDuration).OnComplete(() =>
        {
            mainMenuCanvas.gameObject.SetActive(false);
            SceneManager.LoadScene(gameplaySceneBuildNumber);
        });
    }

    public void OnCreditsButtonClicked()
    {
        mainMenuCanvas.DOFade(0, fadeDuration).OnComplete(() =>
        {
            mainMenuCanvas.gameObject.SetActive(false);
            SceneManager.LoadScene(creditsSceneBuildNumber);
        });
    }

    public void StartCountdown()
    {
        StartCoroutine(this.CountdownCO());
    }

    private IEnumerator CountdownCO()
    {
        for (int i = countdownFrom; i > 0; i--)
        {
            countdownText.SetText(i.ToString());
            yield return new WaitForSeconds(1);
        }

        countdownCanvas.DOFade(0, fadeDuration).OnComplete(() =>
        {
            countdownCanvas.gameObject.SetActive(false);
            LevelManager.Instance.StartLevel();
        });
    }
}
