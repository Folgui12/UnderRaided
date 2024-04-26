using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeToChop : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Tree"))
        {
            Destroy(other.gameObject);
        }
    }
}
