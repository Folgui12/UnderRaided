using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDetect : MonoBehaviour
{
    public GameObject building;

    public float speed;

    // Update is called once per frame
    void Update()
    {
        if (CheckDistance())
            Attack();
        else
        {
            GoToBuilding();
            Debug.Log("Raid");
        }
            
    }

    public void GoToBuilding()
    {
        transform.position = Vector3.MoveTowards(transform.position, building.transform.position, speed * Time.deltaTime);
    }

    public void Attack()
    {
        Debug.Log("Atancando");
    }

    public bool CheckDistance()
    {
        if (Vector3.Distance(transform.position, building.transform.position) < 3)
            return true;
        else
            return false;
    }
}
