using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSF;

public class MovimentoCabecaNovo : MonoBehaviour
{
    public Transform mao;
    MovimentaPersonagem scriptPersonagem;

    Vector3 posicaoMaoOrigem;
    Vector3 posicaoMao;
    float paradoMao, andandoMao;

    Vector3 posicaoCabecaOrigem;
    Vector3 posicaoCabeca;
    float andandoCabeca;

    float intensidade;

    public AudioClip[] passos;
    AudioSource audioSource;
    int indexPassos;

    // Start is called before the first frame update
    void Start()
    {
        scriptPersonagem = GetComponentInParent<MovimentaPersonagem>();
        audioSource = GetComponent<AudioSource>();
        indexPassos = 0;
        posicaoMaoOrigem = mao.localPosition;
        posicaoCabecaOrigem = this.transform.localPosition;
    }

    void CalculaMovimento(float valorTempo, float intensidadeX, float intensidadeY, bool mao)
    {
        if(mao)
        {
            posicaoMao = posicaoMaoOrigem + new Vector3(Mathf.Cos(valorTempo) * intensidadeX, Mathf.Sin(valorTempo * 2) * intensidadeY, 0);
        }
        else
        {
            posicaoCabeca = posicaoCabecaOrigem + new Vector3(0, Mathf.Sin(valorTempo) * intensidadeY, 0);
            if(Mathf.Sin(valorTempo) < -0.95f)
            {
                SomPassos();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //Parado personagem
        if(Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
        {
            CalculaMovimento(paradoMao, 0.01f, 0.01f, true);
            paradoMao += Time.deltaTime;
            mao.localPosition = Vector3.Lerp(mao.localPosition, posicaoMao, Time.deltaTime * 2);

            //cabeca
            this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, Vector3.zero, Time.deltaTime * 10);
        }
        //Correndo
        else if(scriptPersonagem.estaCorrendo)
        {
            CalculaMovimento(andandoMao, 0.07f, 0.07f, true);
            andandoMao = Time.deltaTime * 4;
            mao.localPosition = Vector3.Lerp(mao.localPosition, posicaoMao, Time.deltaTime * 6);

            //cabeca
            CalculaMovimento(andandoCabeca, 0, 0.2f, false);
            andandoCabeca += Time.deltaTime * 15;
            this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, posicaoCabeca, Time.deltaTime * 6);
        }
        //Andando
        else
        {
            CalculaMovimento(andandoMao, 0.035f, 0.035f, true);
            andandoMao = Time.deltaTime * 2;
            mao.localPosition = Vector3.Lerp(mao.localPosition, posicaoMao, Time.deltaTime * 6);

            //cabeca
            CalculaMovimento(andandoCabeca, 0, 0.1f, false);
            andandoCabeca += Time.deltaTime * 10;
            this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, posicaoCabeca, Time.deltaTime * 6);
        }

        //Ondas no movimento
        if(andandoCabeca > Mathf.PI * 2)
        {
            andandoCabeca = andandoCabeca - (Mathf.PI * 2);
        }
    }

        void SomPassos()
        {
            if(!audioSource.isPlaying && scriptPersonagem.estaNoChao) 
            {
                audioSource.clip = passos[indexPassos];
                audioSource.Play();
                indexPassos++;
                if(indexPassos >= 4)
                {
                    indexPassos = 0;
                } 
            }
        }
}
