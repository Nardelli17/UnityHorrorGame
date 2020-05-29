using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InimigoBoss : MonoBehaviour
{
    public NavMeshAgent navMesh;
    public GameObject player;
    public float distanciaDoAtaque = 25;
    public float distanciaDoPlayer;
    public float velocidade = 6;
    Animator anim;
    public int hp = 300;
    public bool estaMorto;
    public Rigidbody rigid;

    public AudioSource audioS;
    public AudioClip[] sons;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        navMesh = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        estaMorto = false;
        audioS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(!estaMorto)
        {
            distanciaDoPlayer = Vector3.Distance(transform.position, player.transform.position);

            VaiAtrasJogador();
            OlhaParaPlayer();

            if(hp <= 0)
            {
                estaMorto = true;
                navMesh.isStopped = true;
                navMesh.enabled = false;
                CorrigeRigEntra();
                anim.CrossFade("Sword And Shield Death", 0.2f);
                transform.gameObject.layer = 10;
                anim.applyRootMotion = true;
                StartCoroutine(SomeMorto());
            }

        }
    }

    IEnumerator SomeMorto()
    {
        yield return new WaitForSeconds(10);
        rigid.isKinematic = false;
        anim.enabled = false;
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);
    }

    void VaiAtrasJogador()
    {
        navMesh.speed = velocidade;

        //inimigo percebe player e segue
        //Revisar essa parte do código
        if(distanciaDoPlayer < 30)
        {
        anim.SetBool("vaiParar",false);
        navMesh.SetDestination(player.transform.position);
        anim.SetBool("joga",false);
        navMesh.isStopped = false;
        CorrigeRigSai();

            if(distanciaDoPlayer <= distanciaDoAtaque)
            {
                navMesh.isStopped = true;
                anim.SetBool("joga",true);
                CorrigeRigEntra();
            }
        }
        if(distanciaDoPlayer >=35)
        {
            anim.SetBool("vaiParar",true);
            anim.SetBool("joga",false);
            navMesh.isStopped = true;
        }
        if(anim.GetBool("vaiParar"))
        {
            anim.SetBool("joga",false);
            navMesh.isStopped = true;
        }
    }


    void OlhaParaPlayer()
    {
        Vector3 direcaoOlha = player.transform.position - transform.position;
        Quaternion rotacao = Quaternion.LookRotation(direcaoOlha);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacao, Time.deltaTime * 300);
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
        rigid.isKinematic = true;
        rigid.velocity = Vector3.zero;
    }

    void CorrigeRigSai()
    {
        rigid.isKinematic = false;
    }

    public void LevouDano(int dano)
    {
        hp -= dano;
    }

    public void SomPassos()
    {
        audioS.PlayOneShot(sons[1]);
    }
}
