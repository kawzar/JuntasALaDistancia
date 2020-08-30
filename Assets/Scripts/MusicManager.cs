using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [SerializeField]
    private AudioClip mainLoop;

    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayMainLoop()
    {
        audioSource.Stop();
        audioSource.clip = mainLoop;
        audioSource.loop = true;
        audioSource.Play();
    }
}
