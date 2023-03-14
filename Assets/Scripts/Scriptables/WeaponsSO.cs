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
    [Range(0, 45f)]
    public float bulletSpread;
    public BulletKeys key;
    public BulletTypes type;
    public FlashLightSO flashLight;
}
