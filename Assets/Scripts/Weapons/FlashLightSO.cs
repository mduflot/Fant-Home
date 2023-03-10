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

    public LightShape shape;
    public LayerMask enemiesMask;
    public int angle;
    public float width;
    public float range;
    public float tickRate;
    public float damagesPerTick;
    public float energyLostPerTick;

}
