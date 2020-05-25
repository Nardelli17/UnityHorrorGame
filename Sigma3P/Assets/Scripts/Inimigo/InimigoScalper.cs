using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CSF;

public class InimigoScalper : MonoBehaviour
{
    public NavMeshAgent navMesh;
    public GameObject player;
    public float distanciaDoAtaque = 2;
    public float distanciaDoPlayer;
    public float velocidade = 4;
    Animator anim;
    public int hp = 100;
    RagDoll ragScript;

    public GameObject objDesliza;
    public bool estaMorto;
    public bool bravo;
    public Renderer render;
    public bool invencivel;

    public AudioClip[] sonsMonstro;
    public AudioSource audioMonstro;

    // Start is called before the first frame update
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        ragScript = GetComponent<RagDoll>();
        render = GetComponentInChildren<Renderer>();
        audioMonstro = GetComponent<AudioSource>();

        invencivel = false;
        estaMorto = false;
        ragScript.DesativaRagdoll();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!estaMorto)
        {
            distanciaDoPlayer = Vector3.Distance(transform.position, player.transform.position);
            VaiAtrasJogador();
            OlhaParaPlayer();

            if(hp <= 50 && !bravo)
            {
                bravo = true;
                anim.ResetTrigger("levouTiro");
                ParaDeAndar();
                anim.CrossFade("Zombie Scream", 0.2f);
                render.material.color = Color.red;
                velocidade = 8;
                
            }

            if (hp <= 0 && !estaMorto)
            {
                MorreSom();
                render.material.color = Color.white;
                objDesliza.SetActive(false);
                estaMorto = true;
                ParaDeAndar();
                navMesh.isStopped = false;
                ragScript.AtivaRagdoll();
                StartCoroutine(ragScript.SomeMorto());
                
            }
        }
    }

    void OlhaParaPlayer()
    {
        Vector3 direcaoOlha = player.transform.position - transform.position;
        Quaternion rotacao = Quaternion.LookRotation(direcaoOlha);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacao, Time.deltaTime * 300);
    }

    void VaiAtrasJogador()
    {
        navMesh.speed = velocidade;

        //inimigo percebe player e segue
        //Revisar essa parte do código
        if(distanciaDoPlayer < 30)
        {
        anim.SetBool("vaiParar",false);
        anim.SetBool("paraAtaque",true);
        anim.SetBool("podeAndar",true);
        if(invencivel == false){
        navMesh.isStopped = false;
        navMesh.SetDestination(player.transform.position);
        }
        else
        {
            navMesh.isStopped = true;
        }

            if(distanciaDoPlayer <= distanciaDoAtaque)
            {
                navMesh.isStopped = true;
                //Debug.Log("Atacando");
                anim.SetTrigger("ataca");
                anim.SetBool("podeAndar",false);
                anim.SetBool("paraAtaque",false);
                CorrigeRigEntra();
            }
        }
        if(distanciaDoPlayer >=35)
        {
            anim.SetBool("podeAndar",false);
            anim.SetBool("vaiParar",true);
            anim.SetBool("paraAtaque",true);
            navMesh.isStopped = true;
        }
        if(anim.GetBool("podeAndar"))
        {
            anim.SetBool("vaiParar",false);
            anim.SetBool("paraAtaque",true);

            if(invencivel == false){
            navMesh.isStopped = false;
            navMesh.SetDestination(player.transform.position);
            }
            else
            {
            navMesh.isStopped = true;
            }

            anim.ResetTrigger("ataca");
            CorrigeRigSai();
        }
        if(anim.GetBool("vaiParar"))
        {
            navMesh.isStopped = true;
            navMesh.SetDestination(player.transform.position);
            anim.ResetTrigger("ataca");
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            CorrigeRigEntra();
        }
    }

    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            CorrigeRigSai();
        }
    }

    void CorrigeRigEntra()
    {
        ragScript.rigid.isKinematic = true;
        ragScript.rigid.velocity = Vector3.zero;
    }

    void CorrigeRigSai()
    {
       ragScript.rigid.isKinematic = false;
    }

     public void LevouDano(int dano)
    {
        int n;

        n = Random.Range(0, 10);

        if(n % 2 == 0 && !bravo)
        {
            anim.SetTrigger("levouTiro"); 
            ParaDeAndar();
        }

        if (!invencivel)
        {
            
            hp -= dano;
        }
    }

    void ParaDeAndar()
    {
        navMesh.isStopped = true;
        anim.SetBool("podeAndar", false);
        CorrigeRigEntra();
    }

    public void DaDanoPlayer()
    {
        player.GetComponent<MovimentaPersonagem>().hp -=10;
    }
    
    public void FicaInvencivel()
    {
        invencivel = true;
        ParaDeAndar();
    }

    public void SaiInvencivel()
    {
        invencivel = false;
        anim.speed = 2;
    }

    public void PassoMonstro()
    {
        audioMonstro.volume = 0.05f;
        audioMonstro.PlayOneShot(sonsMonstro[0]);
    }

    public void SenteDor()
    {
        audioMonstro.volume = 0.05f;
        audioMonstro.clip = sonsMonstro[1];
        audioMonstro.Play();
    }

    public void GritaSom()
    {
        audioMonstro.volume = 0.5f;
        audioMonstro.clip = sonsMonstro[2];
        audioMonstro.Play();
    }

    public void MorreSom()
    {
        audioMonstro.volume = 0.7f;
        audioMonstro.clip = sonsMonstro[3];
        audioMonstro.Play();
    }

}
