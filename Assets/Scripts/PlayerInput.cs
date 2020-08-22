using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }

    public event Action ActionKeyPressed;
    public event Action SwitchPlayerKeyPressed;

    [SerializeField]
    private KeyCode actionKey;

    [SerializeField]
    private KeyCode switchPlayerKey;

    void Update()
    {
        Horizontal = Input.GetAxis("Horizontal");
        Vertical = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(actionKey))
        {
            ActionKeyPressed?.Invoke();
        }
        else if (Input.GetKeyDown(switchPlayerKey))
        {
            SwitchPlayerKeyPressed?.Invoke();
        }
    }
}
