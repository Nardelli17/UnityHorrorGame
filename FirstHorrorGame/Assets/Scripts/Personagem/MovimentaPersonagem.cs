using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSF 
{
    public class MovimentaPersonagem : MonoBehaviour
    {
        public CharacterController controle;
        public float velocidade = 6f;
        public float alturaPulo = 3f;
        public float gravidade = -20f;

        public Transform checaChao;
        public float raioEsfera = 0.4f;
        public LayerMask chaoMask;
        public bool estaNoChao;

        Vector3 velocidadeCai;

        public Transform cameraTransform;
        public bool estaAbaixado;
        public bool levantarBloqueado;
        public float alturaLevantado, alturaAbaixado, posicaoCameraEmPe, posicaoCameraAbaixado;
        RaycastHit hit;

        // Start is called before the first frame update
        void Start()
        {
            controle = GetComponent<CharacterController>();
            estaAbaixado = false;
            cameraTransform = Camera.main.transform;
        }

        // Update is called once per frame
        void Update()
        {
          estaNoChao = Physics.CheckSphere(checaChao.position,raioEsfera, chaoMask);  

        if(estaNoChao && velocidadeCai.y < 0)
        {
            velocidadeCai.y = -2f;
        }
          float x = Input.GetAxis("Horizontal");
          float z = Input.GetAxis("Vertical");

          Vector3 move = (transform.right * x + transform.forward * z).normalized;

          controle.Move(move * velocidade * Time.deltaTime);

          if(Input.GetButtonDown("Jump") && estaNoChao)
          {
              velocidadeCai.y = Mathf.Sqrt(alturaPulo * -2f * gravidade);
          }

            velocidadeCai.y += gravidade * Time.deltaTime;
            controle.Move(velocidadeCai * Time.deltaTime);

            if(estaAbaixado)
            {
                ChecaBloqueioAbaixado();
            }

            if(Input.GetKeyDown(KeyCode.LeftControl))
            {
                Abaixa();
            }
        }

        void Abaixa()
        {
            if(levantarBloqueado || estaNoChao == false)
            {
                return;
            }

            estaAbaixado = !estaAbaixado;
            if(estaAbaixado)
            {
                controle.height = alturaAbaixado;
                cameraTransform.localPosition = new Vector3(0,posicaoCameraAbaixado,0);
            }
            else
            {
                controle.height = alturaLevantado;
                cameraTransform.localPosition = new Vector3(0,posicaoCameraEmPe,0);
            }
        }

        void ChecaBloqueioAbaixado()
        {
            Debug.DrawRay(cameraTransform.position, Vector3.up * 1.1f, Color.red);
            if(Physics.Raycast(cameraTransform.position, Vector3.up, out hit, 1.1f))
            {
                levantarBloqueado = true;
            }
            else
            {
                levantarBloqueado = false;
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(checaChao.position,raioEsfera);
        }
    }
}
