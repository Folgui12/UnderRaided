using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyView : MonoBehaviour
{
    public Animator anim;

    public AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Hago que el modlo del enemigo vea hacia donde camina
    public void LookDir(Vector3 dir)
    {
        if (dir.x == 0 && dir.z == 0) return;

        transform.forward = dir;
    }

    // Comienzo la animación de caminata
    public void StartWalking()
    {
        anim.SetBool("Walking", true);
    }

    // Comienzo la animación de Idle
    public void StayIdle()
    {
        anim.SetBool("Walking", false);
    }

    // Activo la animación de ataque
    public void ActiveAttack()
    {
        anim.SetTrigger("Attack");
    }

    // Activo el sonido de reconocimiento del player
    public void ActiveNoise()
    {
        audio.PlayOneShot(audio.clip);
    }

    // Detecto cuando un Golem chocó con un arbol
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Tree") && gameObject.CompareTag("Golem"))
        {
            Destroy(collision.gameObject);
        }
    }
}
