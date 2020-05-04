using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroiEfeitos : MonoBehaviour
{
    public float tempo = 0;


    void Start()
    {
        Destroy(this.gameObject, tempo);
    }

}
