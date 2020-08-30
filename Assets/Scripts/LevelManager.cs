using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
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

    [SerializeField]
    private CinemachineVirtualCamera[] virtualCameras;

    [SerializeField]
    private float noiseAmplitude = 0.25f;

    [SerializeField]
    private float noiseFrequency = 0.25f;

    private List<CinemachineBasicMultiChannelPerlin> noiseChannels = new List<CinemachineBasicMultiChannelPerlin>();

    public float LowestPlayerPositionY => lowestPlayerPositionY;

    private void Awake()
    {
        Instance = this;
        suhail.PlayerLost += PlayerLost;
        maia.PlayerLost += PlayerLost;

        foreach (CinemachineVirtualCamera camera in virtualCameras)
        {
            noiseChannels.Add(camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>());
        }
    }

    private void Start()
    {
        MusicManager.Instance.PlayMainLoop();
    }

    private void PlayerLost()
    {
        suhail.DisablePlayer();
        maia.DisablePlayer();
        SetCameraNoise(shake: true);
        StartCoroutine(this.DisplayGameOverScreenCO());
    }

    private void OnDestroy()
    {
        suhail.PlayerLost -= PlayerLost;
        maia.PlayerLost -= PlayerLost;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator DisplayGameOverScreenCO()
    {
        yield return new WaitForSeconds(0.75f);

        SetCameraNoise(shake: false);
        UIManager.Instance.DisplayGameOverScreen(true);
    }

    private void SetCameraNoise(bool shake)
    {
        foreach (CinemachineBasicMultiChannelPerlin noise in noiseChannels)
        {
            noise.m_AmplitudeGain = shake ? noiseAmplitude : 0;
            noise.m_FrequencyGain = shake ? noiseFrequency : 0;
        }
    }
}
