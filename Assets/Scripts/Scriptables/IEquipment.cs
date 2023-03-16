using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    GUN,
    LIGHT
}

public abstract class EquipmentSO : ScriptableObject
{
    public string weaponName = "name";
    public string desc = "desc";
    public EquipmentType equipType;
    public Sprite icon;
}
