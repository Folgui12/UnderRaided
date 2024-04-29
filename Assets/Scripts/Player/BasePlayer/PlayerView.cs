using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public Animator anim;
    private RandomMaterial randomMaterial;

    void Awake()
    {
        randomMaterial = GetComponentInChildren<RandomMaterial>();
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        randomMaterial.SetRandomMaterial();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LookDir(float vel)
    {
        transform.Rotate(0, vel * Time.deltaTime, 0);
    }
}
