using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cambiaforma : MonoBehaviour
{
    public MeshFilter Original; //Original
    public MeshFilter FormaDeseada; // a la que se quiere convertir
    public MeshFilter CopiaDelOriginal; //copia de la original


    private void Awake()
    {
        Original = GetComponent<MeshFilter>(); // se llama el meshfilter del original y se guarda en la variable Mf
    }
    void Update()
    {

        Cambiarforma();
    }
    private void Cambiarforma()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Original.mesh = FormaDeseada.mesh; // se cambia el original al deseado al presionar la tecla
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            Original.mesh = CopiaDelOriginal.mesh;// se cambia el deseado al original al dejar de presionar la tecla
        }
    }
}