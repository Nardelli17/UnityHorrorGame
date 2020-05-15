﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSF;

namespace CSF 
{
    public class MovimentaPersonagem : MonoBehaviour
    {

        [Header("Config Personagem")]
        public CharacterController controle;
        public float velocidade = 6f;
        public float alturaPulo = 3f;
        public float gravidade = -20f;
        public bool estaCorrendo;
        public AudioClip[] audiosGerais;
        AudioSource audioPersonagem;
        bool noAr;


        [Header("Verifica Chao")]
        public Transform checaChao;
        public float raioEsfera = 0.4f;
        public LayerMask chaoMask;
        public bool estaNoChao;
        Vector3 velocidadeCai;

        [Header("Verifica Abaixado")]
        public Transform cameraTransform;
        public bool estaAbaixado;
        public bool levantarBloqueado;
        public float alturaLevantado, alturaAbaixado, posicaoCameraEmPe, posicaoCameraAbaixado;
        float velocidadeCorrente = 1f;
        RaycastHit hit;

        [Header("Status Personagem")]
        public float hp = 100;
        public float stamina = 100;
        public bool cansado;
        public Respiracao scriptResp;
       
        

        // Start is called before the first frame update
        void Start()
        {
            cansado = false;
            estaCorrendo = false;
            controle = GetComponent<CharacterController>();
            estaAbaixado = false;
            cameraTransform = Camera.main.transform;
            audioPersonagem  = GetComponent<AudioSource>();
            noAr = false;
        }

        // Update is called once per frame
        void Update()
        {
            Verificacoes();
            MovimentoAbaixa();
            Inputs();
            CondicaoPlayer();
            SomPulo();
            
        }

        void SomPulo()
        {
            if (!estaNoChao)
            {
                noAr = true;
            }

            if (estaNoChao && noAr)
            {
                noAr = false;
                audioPersonagem.clip = audiosGerais[1];
                audioPersonagem.Play();

            }
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
            if(Input.GetKey(KeyCode.LeftShift) && estaNoChao && !estaAbaixado && !cansado)
            {
                estaCorrendo = true;
                velocidade = 9;
                stamina -= 0.3f;
                stamina = Mathf.Clamp(stamina, 0, 100);
            }
            else
            {
                estaCorrendo = false;
                stamina += 0.1f;
                stamina = Mathf.Clamp(stamina, 0, 100);
            }
           
            if(Input.GetButtonDown("Jump") && estaNoChao)
            {
                velocidadeCai.y = Mathf.Sqrt(alturaPulo * -2f * gravidade);
                audioPersonagem.clip = audiosGerais[0];
                audioPersonagem.Play();
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

        void CondicaoPlayer()
        {
            if(stamina == 0)
            {
                cansado = true;
                scriptResp.forcaResp = 5;
            }

            if(stamina > 40)
            {
                cansado = false;
            }
        }

        void OnTriggerStay(Collider col)
        {
            if (col.gameObject.CompareTag("cabecaDesliza"))
            {
                controle.SimpleMove(transform.forward * 1000 * Time.deltaTime);
            }

        }
    }
}
