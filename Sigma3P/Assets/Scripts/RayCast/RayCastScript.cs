using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CSF
{
    public class RayCastScript : MonoBehaviour
    {
        public float distanciaAlvo;
        public GameObject objArrasta,objPega;
        RaycastHit hit;
        public Text textBotao, textInfo;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if(Time.frameCount % 5 == 0)
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 5, Color.red);

                if(Physics.SphereCast(transform.position, 0.01f,transform.TransformDirection(Vector3.forward),out hit,5))
                {
                    distanciaAlvo = hit.distance;
                    if(hit.transform.gameObject.tag == "objArrasta")
                    {
                        objArrasta = hit.transform.gameObject;
                        objPega = null;
                        textBotao.text = "[E]";
                        textInfo.text = "Agarra/Solta";
                    }

                    if(hit.transform.gameObject.tag == "objPega")
                    {
                        objPega = hit.transform.gameObject;
                        objArrasta = null;
                        textBotao.text = "[E]";
                        textInfo.text = "Pegar";
                    }
                }
                else
                {
                    textBotao.text = "";
                    textInfo.text = "";
                    objArrasta = null;
                    objPega = null;
                }
            }
        }
    }
}
