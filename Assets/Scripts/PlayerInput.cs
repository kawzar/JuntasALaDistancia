using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public event Action ActionKeyPressed;
    public event Action SpecialKeyPressed;

    [SerializeField]
    private KeyCode actionKey;

    [SerializeField]
    private KeyCode specialKey;

    void Update()
    {
        if (Input.GetKeyDown(actionKey))
        {
            ActionKeyPressed?.Invoke();
        }
        else if (Input.GetKeyDown(specialKey))
        {
            SpecialKeyPressed?.Invoke();
        }
    }
}
