using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cantpushing : MonoBehaviour
{

    public bool ifinmountain;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("CantDragin"))
        {
            ifinmountain=true;
            
        }
        else
        {
            ifinmountain=false;
        }
    }

}
