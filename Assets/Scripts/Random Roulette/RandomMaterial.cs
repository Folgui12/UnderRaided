using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMaterial : MonoBehaviour
{
    public List<RarityInfo> rarezaMateriales = new List<RarityInfo>();
    public Material bodyMaterial;
    public List<Material> materiales = new List<Material>();
    Dictionary<RarityEnum, float> _pesoDeRarezas;
    private SkinnedMeshRenderer myMaterial;

    void Awake()
    {
        myMaterial = GetComponentInChildren<SkinnedMeshRenderer>();
        _pesoDeRarezas = new Dictionary<RarityEnum, float>();

        for (int i = 0; i < rarezaMateriales.Count; i++)
        {
            _pesoDeRarezas[rarezaMateriales[i].rarity] = rarezaMateriales[i].Weight;
        }
    }

    public RarityEnum GetRandomMaterial()
    {
        return MyRandoms.Roulette(_pesoDeRarezas);
    }


    // Seteo una skin diferente para cada run
    public void SetRandomMaterial()
    {
        Material[] materialesNuevos;

        switch (GetRandomMaterial())
        {
            case RarityEnum.Comun:
                materialesNuevos = new Material[]{bodyMaterial, materiales[0]};
                myMaterial.materials = materialesNuevos;
                break;
            case RarityEnum.Raro:
                materialesNuevos = new Material[]{bodyMaterial, materiales[1]};
                myMaterial.materials = materialesNuevos;
                break;
            case RarityEnum.UltraRaro:
                materialesNuevos = new Material[]{bodyMaterial, materiales[2]};
                myMaterial.materials = materialesNuevos;
                break;

            default:
                break;
        }
    }
}
