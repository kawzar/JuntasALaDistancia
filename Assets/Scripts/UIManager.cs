using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField]
    private CanvasGroup gameOverCanvas;

    [SerializeField]
    private Image gameOverBlackTransition;

    [SerializeField]
    private CanvasGroup mainMenuCanvas;

    [SerializeField]
    private CanvasGroup countdownCanvas;

    [SerializeField]
    private CanvasGroup videoCanvas;

    [SerializeField]
    private Image blackBg;

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

    private int nextScene;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        VideoPlayerManager.Instance.VideoEnded += OnVideoEnded;
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
            Sequence fadeOutSequence = DOTween.Sequence();
            fadeOutSequence.Append(gameOverCanvas.DOFade(0, fadeDuration));
            fadeOutSequence.Join(gameOverBlackTransition.DOColor(Color.black, fadeDuration));
            fadeOutSequence.OnComplete(() => gameOverCanvas.gameObject.SetActive(false));
        }
    }

    public void OnMenuButtonClicked()
    {
        MusicManager.Instance.PlaySelectButtonFx();
        MusicManager.Instance.FadeOutGameMusic(fadeDuration);

        if (gameOverCanvas != null && gameOverCanvas.isActiveAndEnabled)
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
        MusicManager.Instance.PlaySelectButtonFx();
        MusicManager.Instance.FadeOutGameMusic(fadeDuration);
        gameOverCanvas.DOFade(0, fadeDuration).OnComplete(() =>
        {
            gameOverCanvas.gameObject.SetActive(false);
            SceneManager.LoadScene(gameplaySceneBuildNumber);
        });
    }

    public void OnPlayButtonClicked()
    {
        MusicManager.Instance.PlaySelectButtonFx();
        nextScene = gameplaySceneBuildNumber;
        mainMenuCanvas.DOFade(0, fadeDuration).OnComplete(() =>
        {
            mainMenuCanvas.gameObject.SetActive(false);
            videoCanvas.gameObject.SetActive(true);
            videoCanvas.DOFade(1, fadeDuration).OnComplete(() => VideoPlayerManager.Instance.PlayStartCinematic());
        });
    }

    public void OnCreditsButtonClicked()
    {
        MusicManager.Instance.PlayBackButtonFx();
        MusicManager.Instance.FadeOutGameMusic(fadeDuration);

        mainMenuCanvas.DOFade(0, fadeDuration).OnComplete(() =>
        {
            mainMenuCanvas.gameObject.SetActive(false);
            SceneManager.LoadScene(creditsSceneBuildNumber);
        });
    }

    public void OnExitButtonClicked()
    {
        Application.Quit();
    }

    public void StartCountdown()
    {
        StartCoroutine(this.CountdownCO());
    }

    public void ShowEndingCutscene()
    {
        nextScene = creditsSceneBuildNumber;
        blackBg.gameObject.SetActive(true);

        videoCanvas.gameObject.SetActive(true);
        videoCanvas.DOFade(1, fadeDuration).OnComplete(() => VideoPlayerManager.Instance.PlayEndingCinematic());
    }

    private IEnumerator CountdownCO()
    {
        for (int i = countdownFrom; i > 0; i--)
        {
            countdownText.SetText(i.ToString());
            countdownText.transform.DOShakeScale(0.75f);
            MusicManager.Instance.PlaySelectButtonFx();
            yield return new WaitForSeconds(1);
        }

        countdownCanvas.DOFade(0, fadeDuration).OnComplete(() =>
        {
            countdownCanvas.gameObject.SetActive(false);
            LevelManager.Instance.StartLevel();
        });
    }

    private void OnVideoEnded()
    {
        videoCanvas.DOFade(0, fadeDuration).OnComplete(() =>
        {
            videoCanvas.gameObject.SetActive(false);
            if (blackBg != null)
            {
                blackBg.gameObject.SetActive(false);
            }
            SceneManager.LoadScene(nextScene);
        });
    }

    private void OnDestroy()
    {
        VideoPlayerManager.Instance.VideoEnded -= OnVideoEnded;
    }
}
