using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSF;

public class Glock : MonoBehaviour
{
    Animator anim;
    bool estaAtirando;
    RaycastHit hit;

    public GameObject faisca;
    public GameObject buraco;
    public GameObject fumaca;
    public GameObject efeitoTiro;
    public GameObject posEfeitoTiro;
    public GameObject particulaSangue;

    public ParticleSystem rastroBala;

    public AudioSource audioArma;
    public AudioClip[] sonsArma;

    public int carregador = 3;
    public int municao = 17;

    UiManager uiScript;
    MovimentaArma movimentaArmasScript;
    public GameObject posUI;

    public bool automatico;
    public float numeroAleatorioMira;

    public float valorMira;
    // Start is called before the first frame update
    void Start()
    {
        automatico = false;
        estaAtirando = false;
        anim = GetComponent<Animator>();
        audioArma = GetComponent<AudioSource>();
        uiScript = GameObject.FindWithTag("uiManager").GetComponent<UiManager>();
        movimentaArmasScript = GetComponentInParent<MovimentaArma>();
        valorMira = 250;
    }

    // Update is called once per frame
    void Update()
    {
        uiScript.municao.transform.position = Camera.main.WorldToScreenPoint(posUI.transform.position);
        uiScript.municao.text = municao.ToString() + "/" + carregador.ToString();

        ModificaMira();
        if(anim.GetBool("ocorreAcao"))
        {
            return;
        }

       Automatico();
       Atira();
       Recarrega();
       Mira();

    }

    void ModificaMira()
    {
         if (estaAtirando)
        {
            valorMira = Mathf.Lerp(valorMira, 350, Time.deltaTime * 20);
            uiScript.mira.sizeDelta = new Vector2(valorMira, valorMira);
        }
        else
        {
            valorMira = Mathf.Lerp(valorMira, 250, Time.deltaTime * 20);
            uiScript.mira.sizeDelta = new Vector2(valorMira, valorMira);
        }
    }

    void Automatico()
    {
        
        if(Input.GetKeyDown(KeyCode.Q))
        { 
            audioArma.clip = sonsArma[1];
            audioArma.Play();
            automatico = !automatico;

            if(automatico)
            {
                uiScript.imagemModoTiro.sprite = uiScript.spriteModoTiro[1];
            }
            else
            {
                uiScript.imagemModoTiro.sprite = uiScript.spriteModoTiro[0];
            }
        }
    }

    void Atira()
    {
         if(Input.GetButtonDown("Fire1") || automatico?Input.GetButton("Fire1"):false)
        {
            if(!estaAtirando && municao > 0)
            {
                municao--;
                audioArma.clip = sonsArma[0];
                audioArma.Play();
                rastroBala.Play();
                estaAtirando = true;
                StartCoroutine(Atirando());
            }
            else if(!estaAtirando && municao == 0 && carregador > 0)
            {
                anim.Play("RecarregaGlock");
                carregador--;
                municao = 17;
            }
            else if(municao == 0 && carregador == 0)
            {
                audioArma.clip = sonsArma[3];
                audioArma.Play();
            }
        }
    }

    void Recarrega()
    {
         if(Input.GetKeyDown(KeyCode.R) && carregador > 0 && municao < 17)
        {
            anim.Play("RecarregaGlock");
            carregador--;
            municao = 17;
        }
    }

    void Mira()
    {
         if(Input.GetButton("Fire2"))
        {
            anim.SetBool("mira", true);
            posUI.transform.localPosition = new Vector3(0f, 0.1f, -0.2f);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 45, Time.deltaTime * 10);
            uiScript.mira.gameObject.SetActive(false);
            movimentaArmasScript.valor = 0.01f;
            numeroAleatorioMira = 0f;
        }
        else
        {
            anim.SetBool("mira", false);
            posUI.transform.localPosition = new Vector3(-0.02f, 0.1f, -0.2f);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60, Time.deltaTime * 10);
            uiScript.mira.gameObject.SetActive(true);
            movimentaArmasScript.valor = 0.1f;
            numeroAleatorioMira = 0.05f;
        }
    }

    IEnumerator Atirando()
    {
        float screenX = Screen.width / 2;
        float screenY = Screen.height / 2;

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(screenX, screenY));
        anim.Play("AtiraGlock");

        GameObject efeitoTiroObj = Instantiate(efeitoTiro, posEfeitoTiro.transform.position, posEfeitoTiro.transform.rotation);
        efeitoTiroObj.transform.parent = posEfeitoTiro.transform;

        if(Physics.Raycast(new Vector3(ray.origin.x + Random.Range(-numeroAleatorioMira,numeroAleatorioMira), 
        ray.origin.y + Random.Range(-numeroAleatorioMira,numeroAleatorioMira), ray.origin.z)
        , Camera.main.transform.forward, out hit ))
        {
            if(hit.transform.tag == "inimigo")
            {
               if(hit.transform.GetComponent<InimigoScalper>() || hit.transform.GetComponent<InimigoGooser>() || hit.transform.GetComponent<InimigoBoss>())
                {
                    InimigoVerificadorDano();
                }
                else if(hit.rigidbody != null && hit.transform.GetComponentInParent<InimigoScalper>())
                {
                    AdicionaForca(ray, 900);
                }
                else if(hit.rigidbody != null && hit.transform.GetComponentInParent<InimigoGooser>())
                {
                    AdicionaForca(ray, 900);
                }
                else if(hit.rigidbody != null && hit.transform.GetComponentInParent<InimigoBoss>())
                {
                    AdicionaForca(ray, 900);
                }
                GameObject particulaCriada = Instantiate(particulaSangue, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                particulaCriada.transform.parent = hit.transform;
            }
            else
            {
                InstanciaEfeitos();

                if(hit.rigidbody != null)
                {
                    AdicionaForca(ray,400);
                }
            }
            
        }

        yield return new WaitForSeconds(0.7f);
        estaAtirando =  false;
    }

    void InimigoVerificadorDano()
    {
        if (hit.transform.GetComponent<InimigoScalper>())
        {
            hit.transform.GetComponent<InimigoScalper>().LevouDano(20);
        }
        else if (hit.transform.GetComponent<InimigoGooser>())
        {
            hit.transform.GetComponent<InimigoGooser>().LevouDano(20);
        }
        else if (hit.transform.GetComponent<InimigoBoss>())
        {
            hit.transform.GetComponent<InimigoBoss>().LevouDano(20);
        }
    }

    void InstanciaEfeitos()
    {
        Instantiate(faisca, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
        Instantiate(fumaca, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
        GameObject buracoObj = Instantiate(buraco, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
        buracoObj.transform.parent = hit.transform;

    }

    void SomMagazine()
    {
        audioArma.clip = sonsArma[1];
        audioArma.Play();
    }

    void SomUp()
    {
        audioArma.clip = sonsArma[2];
        audioArma.Play();
    }

    void AdicionaForca(Ray ray, float forca)
    {
        Vector3 direcaoBala = ray.direction;
        hit.rigidbody.AddForceAtPosition(direcaoBala * forca, hit.point);
    }

}
