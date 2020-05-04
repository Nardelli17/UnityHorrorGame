using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glock : MonoBehaviour
{
    Animator anim;
    bool estaAtirando;
    RaycastHit hit;

    public GameObject faisca;
    public GameObject buraco;
    public GameObject fumaca;
    public GameObject efeitoTiro;
    public GameObject posEfeitoTiro;
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

        GameObject efeitoTiroObj = Instantiate(efeitoTiro, posEfeitoTiro.transform.position, posEfeitoTiro.transform.rotation);
        efeitoTiroObj.transform.parent = posEfeitoTiro.transform;

        if(Physics.Raycast(new Vector3(ray.origin.x + Random.Range(-0.05f,0.05f), 
        ray.origin.y + Random.Range(-0.05f,0.05f), ray.origin.z), Camera.main.transform.forward, out hit ))
        {
            InstanciaEfeitos();
           if(hit.transform.tag == "objArrasta")
           {
               Vector3 direcaoBala = ray.direction;
               if(hit.rigidbody != null)
               {
                   hit.rigidbody.AddForceAtPosition(direcaoBala * 500, hit.point);
               }
           }
        }

        yield return new WaitForSeconds(1f);
        estaAtirando =  false;
    }

    void InstanciaEfeitos()
    {
        Instantiate(faisca, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
        Instantiate(fumaca, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
        GameObject buracoObj = Instantiate(buraco, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
        buracoObj.transform.parent = hit.transform;

    }
}
