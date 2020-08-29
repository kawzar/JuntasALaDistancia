using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField]
    private float lowestPlayerPositionY = -0.25f;

    [SerializeField]
    private Player suhail;

    [SerializeField]
    private Player maia;

    public float LowestPlayerPositionY => lowestPlayerPositionY;

    private void Awake()
    {
        Instance = this;
        suhail.PlayerLost += PlayerLost;
    }

    private void PlayerLost()
    {
        suhail.DisablePlayer();
        //maia.DisablePlayer();
        UIManager.Instance.DisplayGameOverScreen(true);
    }

    private void OnDestroy()
    {
        suhail.PlayerLost -= PlayerLost;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
