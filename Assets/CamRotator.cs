using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotator : MonoBehaviour
{
    public Vector2 turn;
    public float sensitiviy = .5f;

    // Update is called once per frame
    void Update()
    {
        turn.y += Input.GetAxis("Mouse Y") * sensitiviy;
        turn.x += Input.GetAxis("Mouse X") * sensitiviy;

        transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0);
    }
}
