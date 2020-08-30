using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public event Action ActionKeyPressed;

    [SerializeField]
    private KeyCode actionKey;

    void Update()
    {
        if (Input.GetKeyDown(actionKey))
        {
            ActionKeyPressed?.Invoke();
        }
    }
}
