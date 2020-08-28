using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AudioTool : MonoBehaviour
{
    private static AudioTool Instance
    {
        get
        {
            if (!_instance)
            {
                GameObject go = new GameObject("AudioTool");
                _instance = go.AddComponent<AudioTool>();
            }

            return _instance;
        }
    }

    private static AudioTool _instance;

    private List<AudioSource> _audioSources = new List<AudioSource>();
    private List<AudioSource> _specialAudioSources = new List<AudioSource>();

    private List<Playlist> _currentPlaylists = new List<Playlist>();    
    
    public static void PlaySound(AudioClip clip, bool loop = false, float volume = 1, bool isSpecial = false)
    {
        var audioSource = GetAudioSource(isSpecial);
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.volume = volume;
        audioSource.Play();
    }

    public static AudioSource GetAudioSource(bool isSpecial = false)
    {
        var list = isSpecial ? Instance._specialAudioSources : Instance._audioSources;
        for (int i = 0; i < list.Count; i++)
        {
            var asrc = Instance._audioSources[i];
            if (!asrc.isPlaying)
            {
                return asrc;
            }
        }

        return Instance.CreateAudioSource(isSpecial);
    }

    private AudioSource CreateAudioSource(bool isSpecial)
    {
        AudioSource newSource = Instance.gameObject.AddComponent<AudioSource>();

        if (isSpecial)
        {
            _specialAudioSources.Add(newSource);
        }
        else
        {
            _audioSources.Add(newSource);
        }

        return newSource;
    }

    /// <summary>
    /// Plays a set of sounds one after the other. Returns the queue internal ID in case you want to stop it.
    /// </summary>
    /// <param name="clips"></param>
    /// <returns></returns>
    public static int PlayQueued(bool isSpecial = false, bool loopLast = false, params AudioClip[] clips)
    {
        var playlist = new Playlist(clips, GetAudioSource(isSpecial), loopLast: loopLast);
        playlist.source.Play();
        playlist.Start();
        Instance._currentPlaylists.Add(playlist);

        return Instance._currentPlaylists.Count - 1;
    }

    public static void StopQueue(int queueId, bool fadeOut = false, float fadeOutTime = 1)
    {
        if (queueId < 0 || queueId >= Instance._currentPlaylists.Count)
        {
            Debug.LogWarning($"Wrong queue id {queueId}. It's possible that the playlist has already finished.");
            return;
        }

        Instance._currentPlaylists[queueId].Stop(fadeOut, fadeOutTime);
    }

    public static void SetMasterVolume(float volume)
    {
        Instance._audioSources.ForEach(x => x.volume = volume);
    }

    private void Update()
    {
        UpdatePlaylists();
    }

    private void UpdatePlaylists()
    {
        for (int i = 0; i < _currentPlaylists.Count; i++)
        {
            _currentPlaylists[i].Update();

            if (_currentPlaylists[i].Finished)
            {
                RemovePlaylistAtIndex(i);
            }
        }
    }

    private void RemovePlaylistAtIndex(int i)
    {
        Debug.Log($"Playlist removed at {i}");
        _currentPlaylists.RemoveAt(i);
    }
}

[System.Serializable]
public class Playlist
{
    public readonly AudioClip[] clips;
    public readonly AudioSource source;

    public bool Finished { get; private set; }

    private int _currentClip;
    private bool _started;

    private bool _loopLast;

    private Tween _volumeTween;

    public Playlist(AudioClip[] clips, AudioSource source, bool startImmediately = true, bool loopLast = false)
    {
        this.clips = clips;
        this.source = source;
        this._loopLast = loopLast;

        source.loop = false;

        Finished = false;

        _started = startImmediately;
    }

    public void Start()
    {
        _started = true;
    }

    public void Stop(bool fadeOut = false, float fadeOutTime = 3)
    {
        if (fadeOut)
        {
            _volumeTween?.Kill();

            fadeOutTime = Mathf.Min(fadeOutTime, source.clip.length);
            _volumeTween = source.DOFade(0, fadeOutTime).OnComplete(() => Finished = true);                        

            return;
        }

        source.Stop();
        Finished = true;
    }

    /// <summary>
    /// To be called on Update. 
    /// </summary>
    /// <returns></returns>
    public void Update()
    {
        if (!_started) return;
        
        if (_currentClip < clips.Length)
        {
            if (!source.isPlaying)
            {
                source.clip = clips[_currentClip];
                source.Play();
                _currentClip++;

                if (_currentClip == clips.Length && _loopLast)
                {
                    source.loop = true;
                }
            }
        }
        else
        {
            Debug.Log($"CurrentClip: {_currentClip}. Total: {clips.Length}");
            //Debug.Log("FINISHED");
            Finished = true;
        }        
    }
}

