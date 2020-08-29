using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cambiocollider : MonoBehaviour
{
    public Collider Original;
    public Collider Formadeseada; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CambiarCollider();
    }

    private void CambiarCollider()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Original.enabled = false;
            Formadeseada.enabled = true;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            Original.enabled = true;
            Formadeseada.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Estoy chocando uwu ");
    }
}
