using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public Animator anim;
    private RandomMaterial randomMaterial;
    private PlayerModel _model;

    void Awake()
    {
        randomMaterial = GetComponentInChildren<RandomMaterial>();
        _model = GetComponent<PlayerModel>();
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        randomMaterial.SetRandomMaterial();
    }
}
