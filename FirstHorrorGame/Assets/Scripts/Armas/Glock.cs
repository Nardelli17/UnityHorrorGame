using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glock : MonoBehaviour
{
    Animator anim;
    bool estaAtirando;
    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        estaAtirando = false;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            if(!estaAtirando)
            {
                estaAtirando = true;
                StartCoroutine(Atirando());
            }
        }
    }

    IEnumerator Atirando()
    {
        float screenX = Screen.width / 2;
        float screenY = Screen.height / 2;

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(screenX, screenY));
        anim.Play("AtiraGlock");

        if(Physics.SphereCast(ray, 0.1f, out hit))
        {
           if(hit.transform.tag == "objArrasta")
           {
               Vector3 direcaoBala = ray.direction;
               if(hit.rigidbody != null)
               {
                   hit.rigidbody.AddForceAtPosition(direcaoBala * 500, hit.point);
               }
           }
        }

        yield return new WaitForSeconds(0.3f);
        estaAtirando =  false;
    }
}
