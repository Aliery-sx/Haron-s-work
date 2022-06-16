using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterManager : MonoBehaviour
{
    public float wavesHeight = 1.3f;
    public float wavesFrequency = 0.02f;
    public float wavesSpeed = 0.03f;
    public Transform water;
    Material waterMat;
    Texture2D waterDisplacement;
    void Awake()
    {
        SetVariables();
    }
    void SetVariables()
    {
        waterMat = water.GetComponent<Renderer>().sharedMaterial;
        waterDisplacement = (Texture2D)waterMat.GetTexture("Water_Displacement");
    }

    
    public float WaterHeightAtPosition(Vector3 position)
    {
        return water.position.y + waterDisplacement.GetPixelBilinear(position.x * wavesFrequency, position.z * wavesFrequency + Time.time * wavesSpeed).g * (wavesHeight / 100) * water.localScale.x;
    }

    private void OnValidate()
    {
        if (!waterMat)
            SetVariables();
        UpdateMaterials();
    }
    void UpdateMaterials()
    {
        waterMat.SetFloat("Waves_Speed",wavesSpeed);
        waterMat.SetFloat("Waves_Frequency",wavesFrequency);
        waterMat.SetFloat("Waves_Height",wavesHeight);
    }
}
