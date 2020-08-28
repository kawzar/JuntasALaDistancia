using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField]
    private CanvasGroup gameOverCanvas;

    [SerializeField]
    private float fadeDuration;

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
        // TODO
    }

    public void OnRestartButtonClicked()
    {
        gameOverCanvas.DOFade(0, fadeDuration).OnComplete(() =>
        {
            gameOverCanvas.gameObject.SetActive(false);
            LevelManager.Instance.RestartLevel();
        });
    }
}
