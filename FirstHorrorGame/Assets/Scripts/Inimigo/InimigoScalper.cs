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
    public float velocidade = 5;
    Animator anim;
    public int hp = 100;
    RagDoll ragScript;

    public bool estaMorto;

    // Start is called before the first frame update
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        ragScript = GetComponent<RagDoll>();

        estaMorto = false;
        ragScript.DesativaRagdoll();
        
    }

    // Update is called once per frame
    void Update()
    {
        distanciaDoPlayer = Vector3.Distance(transform.position, player.transform.position);
        VaiAtrasJogador();
        OlhaParaPlayer();
        
        if (hp <= 0 && !estaMorto)
        {
            estaMorto = true;
            ParaDeAndar();
            ragScript.AtivaRagdoll();
            this.enabled = false;
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
        if(distanciaDoPlayer < 10 )
        {
        anim.SetBool("vaiParar",false);
        anim.SetBool("paraAtaque",true);
        anim.SetBool("podeAndar",true);
        navMesh.isStopped = false;
        navMesh.SetDestination(player.transform.position);

            if(distanciaDoPlayer <= distanciaDoAtaque)
            {
                navMesh.isStopped = true;
                Debug.Log("Atacando");
                anim.SetTrigger("ataca");
                anim.SetBool("podeAndar",false);
                anim.SetBool("paraAtaque",false);
                CorrigeRigEntra();
            }
        }
        if(distanciaDoPlayer >=15)
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
            navMesh.isStopped = false;
            navMesh.SetDestination(player.transform.position);
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
        hp -= dano;
    }

    void ParaDeAndar()
    {
        navMesh.isStopped = true;   
        anim.SetBool("podeAndar", false);
        CorrigeRigEntra();
    }

    
}
