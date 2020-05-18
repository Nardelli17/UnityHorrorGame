using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMouse : MonoBehaviour
{
    public Material selecionado, naoSelecionado;
    Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseEnter()
    {
        rend.material = selecionado;
    }

    void OnMouseExit()
    {
        rend.material = naoSelecionado;
    }
}
