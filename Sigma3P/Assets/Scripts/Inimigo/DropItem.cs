using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public GameObject itemDrop;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     public void Dropa()
    {
        int n = Random.Range(0, 10);

        if(n > 2)
        {
            GameObject item = Instantiate(itemDrop, transform);
            item.GetComponent<Rigidbody>().AddForce((transform.up * 5 + transform.forward * 3), ForceMode.Impulse);
            item.transform.parent = null;
        }

    }
}
