using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InimigoScalper : MonoBehaviour
{
    public NavMeshAgent navMesh;
    public GameObject player;
    public float distanciaDoAtaque = 2;
    public float distanciaDoPlayer;
    public float velocidade = 5;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        distanciaDoPlayer = Vector3.Distance(transform.position, player.transform.position);
        VaiAtrasJogador();
    }

    void VaiAtrasJogador()
    {
        navMesh.speed = velocidade;

        //inimigo percebe player e segue
        //Revisar essa parte do código
        if(distanciaDoPlayer < 10)
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
            }
        }
        if(distanciaDoPlayer >=11)
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
        }
        if(anim.GetBool("vaiParar"))
        {
            navMesh.isStopped = true;
            navMesh.SetDestination(player.transform.position);
            anim.ResetTrigger("ataca");
        }
    }
}
