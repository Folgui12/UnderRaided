using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRandomFlashlightColor : MonoBehaviour
{
    public Light flashlight; 
    public List<RarityInfo> rarezaColores = new List<RarityInfo>();
    public List<Color> colores = new List<Color>();
    Dictionary<RarityEnum, float> _pesoDeRarezas;

    void Awake()
    {
        _pesoDeRarezas = new Dictionary<RarityEnum, float>();

        for (int i = 0; i < rarezaColores.Count; i++)
        {
            _pesoDeRarezas[rarezaColores[i].rarity] = rarezaColores[i].Weight;
        }
    }

    // Pedimos Rareza
    public RarityEnum GetRandomColor()
    {
        switch(GameManager.Instance.keyCount)
        {
            case 1:
                _pesoDeRarezas[RarityEnum.Comun] = 60;
                _pesoDeRarezas[RarityEnum.Raro] = 20;
                _pesoDeRarezas[RarityEnum.UltraRaro] = 20; 
                break; 
            case 2:
                _pesoDeRarezas[RarityEnum.Comun] = 40;
                _pesoDeRarezas[RarityEnum.Raro] = 30;
                _pesoDeRarezas[RarityEnum.UltraRaro] = 30; 
                break; 
            case 3:
                _pesoDeRarezas[RarityEnum.Comun] = 30;
                _pesoDeRarezas[RarityEnum.Raro] = 30;
                _pesoDeRarezas[RarityEnum.UltraRaro] = 40; 
                break; 

            default:
                break; 
        }

        return MyRandoms.Roulette(_pesoDeRarezas);
    }

    public void SetRandomColor()
    {
        switch (GetRandomColor())
        {
            case RarityEnum.Comun:
                flashlight.color = colores[0];
                break;
                
            case RarityEnum.Raro:
                flashlight.color = colores[1];
                break;

            case RarityEnum.UltraRaro:
                flashlight.color = colores[2];
                break;

            default:
                break;
        }
    }
}
