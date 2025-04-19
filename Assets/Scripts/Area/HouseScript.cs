using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseScript : MonoBehaviour
{
    Vector3 originPos;

    [SerializeField]
    GameObject particalSmoke;

    private void Start()
    {
        originPos = transform.position;
    }

    private void OnEnable()
    {
        transform.position = transform.position + new Vector3(0,50,0);
    }

    private void SetPos()
    {
        transform.position = originPos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision != null && (collision.gameObject.tag == "Area"|| collision.gameObject.tag =="CantDragin" ))
        {
            Instantiate(particalSmoke, transform);
            GetComponent<Rigidbody>().isKinematic = true;
        }
        
        
        
    }   
    
    
}
