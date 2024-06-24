using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offSet;

    [SerializeField] private float sensitivity;

    private float x;
    private float y;    

    // Update is called once per frame
    void Update()
    {
        x += Input.GetAxis("Mouse X") * sensitivity;
        y += Input.GetAxis("Mouse Y") * -sensitivity;

        transform.localEulerAngles = new Vector3(y, x, 0);
        //transform.position = player.position + offSet;
        player.transform.localEulerAngles = new Vector3(0, x, 0);
        
    }
}
