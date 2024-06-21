using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashlightPower : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Light flahslight;
    [SerializeField] private Image lightTime; 
    [SerializeField] private Transform lightPoint;

    private RaycastHit hit;

    private float timeToUseFlahslight = 10;

    void Start()
    {
        lightTime.fillAmount = timeToUseFlahslight;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1) && timeToUseFlahslight > 0)
        {
            if(Physics.Raycast(lightPoint.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(lightPoint.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                Debug.Log("Did Hit");

                flahslight.intensity = 2;
                flahslight.spotAngle = 20;
            }
            timeToUseFlahslight -= Time.deltaTime;
        }

        if (Input.GetMouseButtonUp(1))
        {
            flahslight.intensity = 1.25f;
            flahslight.spotAngle = 65;
        }

        lightTime.fillAmount = timeToUseFlahslight / 10;
    }
}
