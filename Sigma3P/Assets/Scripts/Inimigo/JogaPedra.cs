using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSF;

public class JogaPedra : MonoBehaviour
{
    Rigidbody rigid;
    public float hVelocidade = 15;
    public float vVelocidade = 4;
    GameObject player;
    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        Destroy(this.gameObject, 6);
    }

    public void Joga()
    {
        rigid = GetComponent<Rigidbody>();
        Vector3 targetForca = transform.forward * hVelocidade;
        targetForca += transform.up * vVelocidade;
        rigid.AddForce(targetForca, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            player.GetComponent<MovimentaPersonagem>().hp -= 30;
        }
        Destroy(this.gameObject);
    }
    


}
