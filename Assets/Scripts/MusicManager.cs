using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Clips")]
    [SerializeField]
    private AudioClip mainLoop;

    [SerializeField]
    private AudioClip selectButtonFx;

    [SerializeField]
    private AudioClip backButtonFx;

    [SerializeField]
    private AudioClip gameOverFx;

    [Header("Audio Sources")]
    [SerializeField]
    private AudioSource musicAudioSource;

    [SerializeField]
    private AudioSource fxAudioSource;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayMainLoop()
    {
        musicAudioSource.Stop();
        musicAudioSource.clip = mainLoop;
        musicAudioSource.loop = true;
        musicAudioSource.volume = 0;
        musicAudioSource.Play();
        musicAudioSource.DOFade(1, 1f);
    }

    public void PlaySelectButtonFx()
    {
        fxAudioSource.clip = selectButtonFx;
        fxAudioSource.Play();
    }

    public void PlayBackButtonFx()
    {
        fxAudioSource.clip = backButtonFx;
        fxAudioSource.Play();
    }

    public void PlayGameOverFx()
    {
        fxAudioSource.clip = backButtonFx;
        fxAudioSource.Play();
    }

    public void FadeOutGameMusic(float fadeDuration)
    {
        musicAudioSource.DOFade(0, fadeDuration);
    }
}