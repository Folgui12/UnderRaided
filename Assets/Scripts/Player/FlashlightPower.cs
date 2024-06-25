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
    [SerializeField] private AudioClip audioFlash; 

    private RaycastHit hit;

    private RaycastHit previousHitInfo; 

    private float timeToUseFlahslight = 10;

    private AudioSource audio;

    private SetRandomFlashlightColor randomColor; 
    private bool changeColorOneTime = true;

    void Start()
    {
        lightTime.fillAmount = timeToUseFlahslight;
        audio = GetComponent<AudioSource>();
        randomColor = GetComponent<SetRandomFlashlightColor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1) && timeToUseFlahslight > 0)
        {
            if(!audio.isPlaying)
                audio.PlayOneShot(audioFlash); 

            if(changeColorOneTime)
            {
                randomColor.SetRandomColor();
                changeColorOneTime = false;
            }
            

            if(Physics.Raycast(lightPoint.position, transform.TransformDirection(Vector3.forward),out hit, Mathf.Infinity, layerMask))
            {
                previousHitInfo = hit;

                hit.collider.gameObject.GetComponent<BaseEnemyController>().SetToEvade = true;
            }

            flahslight.intensity = 2;
            flahslight.spotAngle = 20;

            timeToUseFlahslight -= Time.deltaTime;
        }
        else
        {
            if(previousHitInfo.collider != null)
            {   
                previousHitInfo.collider.gameObject.GetComponent<BaseEnemyController>().SetToEvade = false;
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            changeColorOneTime = true; 
            flahslight.intensity = 1.25f;
            flahslight.spotAngle = 65;
        }

        lightTime.fillAmount = timeToUseFlahslight / 10;
    }
}
