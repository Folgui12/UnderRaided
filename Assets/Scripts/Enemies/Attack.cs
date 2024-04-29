using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Attack : MonoBehaviour
{
    // Compruebo que el enemigo golpeó con el brazo/puño al player y modifico su vida
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerModel>().life -= 5;
        }
    }
}
