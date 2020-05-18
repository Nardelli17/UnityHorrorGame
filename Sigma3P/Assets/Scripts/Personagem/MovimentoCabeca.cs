using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSF;

namespace CSF 
{
    public class MovimentoCabeca : MonoBehaviour
    {
        private float tempo = 0.0f;
        public float velocidade = 0.05f;
        public float forca = 0.1f;
        public float pontoDeOrigem = 0.0f;

        float cortaOnda;
        float horizontal;
        float vertical;
        Vector3 salvaPosicao;

        AudioSource audioSource;
        public AudioClip[] audioClip;
        public int indexPassos;

        MovimentaPersonagem scriptMovimenta;

        void Start()
        {
            scriptMovimenta = GetComponentInParent<MovimentaPersonagem>();
            audioSource = GetComponent<AudioSource>();
            indexPassos = 0;
        }

        // Update is called once per frame
        void Update()
        {
            cortaOnda = 0.0f;
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            salvaPosicao = transform.localPosition;

            if(Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
            {
                tempo = 0.0f;
            }
            else
            {
                cortaOnda = Mathf.Sin(tempo);
                tempo = tempo + velocidade;

                if(tempo > Mathf.PI * 2)
                {
                    tempo = tempo - (Mathf.PI * 2);
                }
            }

            if(cortaOnda != 0)
            {
                float mudaMovimentacao = cortaOnda * forca;
                float eixosTotais = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
                eixosTotais = Mathf.Clamp(eixosTotais, 0.0f, 1.0f);
                mudaMovimentacao = eixosTotais * mudaMovimentacao;
                salvaPosicao.y = pontoDeOrigem + mudaMovimentacao;
            }
            else
            {
                salvaPosicao.y = pontoDeOrigem;
            }

            transform.localPosition = salvaPosicao;

            SomPassos();
            AtualizaCabeca();
        }


        void SomPassos()
        {
            if(cortaOnda <= -0.95f && !audioSource.isPlaying && scriptMovimenta.estaNoChao) 
            {
                audioSource.clip = audioClip[indexPassos];
                audioSource.Play();
                indexPassos++;
                if(indexPassos >= 4)
                {
                    indexPassos = 0;
                }
                
            }
        }

        void AtualizaCabeca()
        {
            if (scriptMovimenta.estaCorrendo)
            {
                velocidade = 0.25f;
                forca = 0.25f;
            }
            else if (scriptMovimenta.estaAbaixado)
            {
                velocidade = 0.15f;
                forca = 0.11f;
            }
            else
            {
                velocidade = 0.18f;
                forca = 0.15f;
            }
        }

        
    }
}

