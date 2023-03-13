using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FlashLight", menuName = "Scriptables/FlashLight", order = 1)]
public class FlashLightSO : ScriptableObject
{
    public enum LightShape
    {
        BOX,
        CONIC
    }

    [Header("Shapes")]
    public LightShape shape;
    [Range(0, 180)]
    public int angle;
    [Range(0.1f, 5)]
    public float width;
    [Range(0.1f, 35)]
    public float range;
    
    [Header("Statistics")]
    public float tickRate;
    public float damagesPerTick;
    public float energyLostPerTick;

}
