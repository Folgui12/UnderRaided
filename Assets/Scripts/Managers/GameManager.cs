using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void GameFinish()
    {
        if(keyCount >= 3)
        {
            SceneManager.LoadScene(3);
        }
    }

    public void GoToLoseScreen()
    {
        SceneManager.LoadScene(2);
    }

}
