using System;
using Unity.Entities;
using UnityEngine;

public enum BulletKeys
{
    Chain,
    BigBertha
}

public enum BulletTypes
{
    Classic,
    Explosive
}

[CreateAssetMenu(fileName = "WeaponStats", menuName = "Scriptables/WeaponStats", order = 2)]
public class WeaponsSO: EquipmentSO
{
    public float bulletSpeed;
    public float reloadTime;
    public BulletKeys key;
    public BulletTypes type;
    public FlashLightSO flashLight;
}
