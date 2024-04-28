using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRandoms : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float Range(float max, float min)
    {
        return min + Random.value * (max - min);
    }

    public T Rulette<T>(Dictionary<T, float> items)
    {
        float total = 0;

        foreach (var item in items)
        {
            total += item.Value;
        }

        var random = Range(0, total);

        foreach (var item in items)
        {
            if(random <= item.Value)
            {
                return item.Key;
            }
            else
            {
                random -= item.Value;
            }
        }

        return default(T);
    }
}
