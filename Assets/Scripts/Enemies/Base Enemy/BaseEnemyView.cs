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

    public void LookDir(Vector3 dir)
    {
        if (dir.x == 0 && dir.z == 0) return;

        transform.forward = dir;
    }

    public void StartWalking()
    {
        anim.SetBool("Walking", true);
    }

    public void StayIdle()
    {
        anim.SetBool("Walking", false);
    }

    public void ActiveAttack()
    {
        anim.SetTrigger("Attack");
    }

    public void ActiveNoise()
    {
        audio.PlayOneShot(audio.clip);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Tree") && gameObject.CompareTag("Golem"))
        {
            Destroy(collision.gameObject);
        }
    }
}
