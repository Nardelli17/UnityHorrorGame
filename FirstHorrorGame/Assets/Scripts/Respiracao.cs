using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respiracao : MonoBehaviour
{
    public bool estaInspirando = true;
    public float minAltura = -0.035f, maxAltura = 0.035f;

    [Range(0f,5f)]
    public float forcaResp = 1f;

    float movimento;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(estaInspirando)
        {
            movimento = Mathf.Lerp(movimento, maxAltura, Time.deltaTime * 1 * forcaResp);
            transform.localPosition = new Vector3(transform.localPosition.x, movimento, transform.localPosition.z);
            if(movimento >= maxAltura - 0.01f)
            {
                estaInspirando = !estaInspirando;
            }
        }
        else
        {
            movimento = Mathf.Lerp(movimento, minAltura, Time.deltaTime * 1 * forcaResp);
            transform.localPosition = new Vector3(transform.localPosition.x, movimento, transform.localPosition.z);
            if(movimento <= minAltura + 0.01f)
            {
                estaInspirando = !estaInspirando;
            }
        }

        if(forcaResp != 0)
        {
            forcaResp = Mathf.Lerp(forcaResp, 1f, Time.deltaTime * 0.02f);
        }
    }
}
