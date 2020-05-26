using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSF
{
    [RequireComponent(typeof(RayCastScript))]
    public class AcoesRayCast : MonoBehaviour
    {
        RayCastScript rayCastScript;
        public bool pegou;
        float distancia;
        public GameObject salvaObjeto;
        MovimentaPersonagem scriptMovimenta;

        // Start is called before the first frame update
        void Start()
        {
            rayCastScript = GetComponent<RayCastScript>();
            pegou = false;
            scriptMovimenta = GetComponentInParent<MovimentaPersonagem>();
        }

        // Update is called once per frame
        void Update()
        {
            distancia = rayCastScript.distanciaAlvo;

            if(distancia <= 3)
            {
                if(Input.GetKeyDown(KeyCode.E) && rayCastScript.objPega != null)
                {
                    Pegar();
                }
                if(Input.GetKeyDown(KeyCode.E) && rayCastScript.objArrasta != null)
                {
                    if(!pegou)
                    {
                        pegou = true;
                        Arrastar();
                    }
                    else
                    {
                        pegou = false;
                        Soltar();
                    }
                }
            }

        }
        void Arrastar()
        {
            rayCastScript.objArrasta.GetComponent<Rigidbody>().isKinematic = true;
            rayCastScript.objArrasta.GetComponent<Rigidbody>().useGravity = false;
            rayCastScript.objArrasta.transform.SetParent(transform);
            rayCastScript.objArrasta.transform.localPosition = new Vector3(0,0,2f);
            rayCastScript.objArrasta.transform.localRotation = Quaternion.Euler(0,0,0);
        }

        void Soltar()
        {
            rayCastScript.objArrasta.transform.localPosition = new Vector3(0,0,2f);
            rayCastScript.objArrasta.transform.SetParent(null);
            rayCastScript.objArrasta.GetComponent<Rigidbody>().isKinematic = false;
            rayCastScript.objArrasta.GetComponent<Rigidbody>().useGravity = true;
        }

        void Pegar()
        {
            //ganha vida
            scriptMovimenta.hp += 50;
            scriptMovimenta.hp = Mathf.Clamp(scriptMovimenta.hp, 0, 100);

            //ganha municao e carregador
            GetComponentInChildren<Glock>().carregador = 3;
            GetComponentInChildren<Glock>().municao = 17;
            Destroy(rayCastScript.objPega);
        }
    }
}
