using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerModel : MonoBehaviour
{
    [SerializeField] private float speed; 

    public float life;

    private Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(Vector3 dir)
    {
        dir *= speed;
        dir.y = _rb.velocity.y;
        _rb.velocity = dir;
    }

    public void TakeHit()
    {
        life--;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("FinishLine"))
        {
            SceneManager.LoadScene("WinScreen");
        }
    }
}
