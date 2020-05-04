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
        float velocidadeCorrente = 1f;
        RaycastHit hit;
        public bool estaCorrendo;

        // Start is called before the first frame update
        void Start()
        {
            estaCorrendo = false;
            controle = GetComponent<CharacterController>();
            estaAbaixado = false;
            cameraTransform = Camera.main.transform;
        }

        // Update is called once per frame
        void Update()
        {
            Verificacoes();
            MovimentoAbaixa();
            Inputs();
            
        }

        void Verificacoes()
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

            velocidadeCai.y += gravidade * Time.deltaTime;
            controle.Move(velocidadeCai * Time.deltaTime);

        }
        void Inputs()
        {
            if(Input.GetKey(KeyCode.LeftShift) && estaNoChao && !estaAbaixado)
            {
                estaCorrendo = true;
                velocidade = 9;
            }
            else
            {
                estaCorrendo = false;
            }
           
            if(Input.GetButtonDown("Jump") && estaNoChao)
            {
                velocidadeCai.y = Mathf.Sqrt(alturaPulo * -2f * gravidade);
            }
            if(Input.GetKeyDown(KeyCode.LeftControl))
            {
                Abaixa();
            }
        }

        void MovimentoAbaixa()
        {
            controle.center = Vector3.down * (alturaLevantado - controle.height) / 2f;

            if (estaAbaixado)
            {
                controle.height = Mathf.Lerp(controle.height, alturaAbaixado, Time.deltaTime * 3);
                float novoY = Mathf.SmoothDamp(cameraTransform.localPosition.y, posicaoCameraAbaixado, ref velocidadeCorrente, Time.deltaTime * 3);
                cameraTransform.localPosition = new Vector3(0, novoY, 0);
                velocidade = 3f;
                ChecaBloqueioAbaixado();
            }
            else
            {
                controle.height = Mathf.Lerp(controle.height, alturaLevantado, Time.deltaTime * 3);
                float novoY = Mathf.SmoothDamp(cameraTransform.localPosition.y, posicaoCameraEmPe, ref velocidadeCorrente, Time.deltaTime * 3);
                cameraTransform.localPosition = new Vector3(0, novoY, 0);
                velocidade = 6f;
            }
        }
        
        void Abaixa()
        {
            if(levantarBloqueado || estaNoChao == false)
            {
                return;
            }

            estaAbaixado = !estaAbaixado;
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
