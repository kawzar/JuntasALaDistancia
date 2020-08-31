using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerManager : MonoBehaviour
{
    public event Action VideoEnded;

    public static VideoPlayerManager Instance { get; private set; }

    [SerializeField]
    private VideoClip startCinematic;

    [SerializeField]
    private VideoClip endingCinematic;

    [SerializeField]
    private VideoPlayer videoPlayer;

    private bool startedPlayingVideo = false;

    private void Awake()
    {
        Instance = this;
        startedPlayingVideo = false;

        videoPlayer.prepareCompleted += VideoPlayer_prepareCompleted;
    }

    private void Update()
    {
        if (startedPlayingVideo && videoPlayer.frame > 0 && !videoPlayer.isPlaying)
        {
            VideoEnded?.Invoke();
        }
    }

    public void PlayStartCinematic()
    {
        videoPlayer.clip = startCinematic;
        //videoPlayer.Prepare();

        videoPlayer.Play();
        MusicManager.Instance.PlayIntro();
    }

    private void VideoPlayer_prepareCompleted(VideoPlayer source)
    {
        videoPlayer.Play();
        startedPlayingVideo = true;
    }

    public void PlayEndingCinematic()
    {
        videoPlayer.clip = endingCinematic;

        videoPlayer.Play();
        MusicManager.Instance.PlayEnding();
    }

    private void OnDestroy()
    {
        videoPlayer.prepareCompleted -= VideoPlayer_prepareCompleted;
    }
}
