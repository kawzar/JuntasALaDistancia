using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public event Action ActionKeyPressed;
    public event Action SpecialKeyPressed;
    ParticleSystem mparticle;

    [SerializeField]
    private KeyCode actionKey;

    [SerializeField]
    private KeyCode specialKey;
    void Awake()
    {
        mparticle = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        if (Input.GetKeyDown(actionKey))
        {
            ActionKeyPressed?.Invoke();
            mparticle.Play();
        }
        else if (Input.GetKeyDown(specialKey))
        {
            SpecialKeyPressed?.Invoke();
            mparticle.Play();
        }
    }
}
