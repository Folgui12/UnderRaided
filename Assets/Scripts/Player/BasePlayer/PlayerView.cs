using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public Animator anim;
    private RandomMaterial randomMaterial;
    private AudioSource source;

    void Awake()
    {
        randomMaterial = GetComponentInChildren<RandomMaterial>();
        source = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        randomMaterial.SetRandomMaterial();
    }

    public void StepNoise()
    {
        if(!source.isPlaying)
            source.PlayOneShot(source.clip);
    }
}
