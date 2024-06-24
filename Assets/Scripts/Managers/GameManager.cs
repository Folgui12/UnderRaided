using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int keyCount; 

    public bool canPlayerScape;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(this);
    }


}
