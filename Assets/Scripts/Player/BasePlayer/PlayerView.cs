using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public Animator anim;
    private Rigidbody _rb;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LookDir(float vel)
    {
        transform.Rotate(0, vel * Time.deltaTime, 0);
    }

    public void Death()
    {
        
    }
}
