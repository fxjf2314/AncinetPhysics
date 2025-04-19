using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        GetComponent<Animator>().enabled = true;
        Destroy(gameObject,1f);
    }
}
