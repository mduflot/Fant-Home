using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Entities;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public enum BulletKeys
{
    Chain,
    BigBertha
}

public enum BulletTypes
{
    Classic,
    Explosive,
    Triple
}

[CreateAssetMenu(fileName = "WeaponStats", menuName = "Scriptables/WeaponStats", order = 2)]
public class WeaponsSO: EquipmentSO
{
    public int damage;
    public float bulletSpeed;
    public float reloadTime;
    [Range(0, 45f)]
    public float bulletSpread;
    public BulletKeys key;
    public BulletTypes type;
    public FlashLightSO flashLight;
    
    
}

[CustomEditor(typeof(WeaponsSO)), CanEditMultipleObjects]
public class WeaponEditor : Editor
{
    private SerializedProperty iDamage;
    private SerializedProperty fSpeed;
    private SerializedProperty fReload;
    private SerializedProperty fSpread;
    private SerializedProperty bkKey;
    private SerializedProperty btType;
    private SerializedProperty fsoFlashlight;

    private void OnEnable()
    {
        iDamage = serializedObject.FindProperty("damage");
        iDamage = serializedObject.FindProperty("bulletSpeed");
        iDamage = serializedObject.FindProperty("reloadTime");
        iDamage = serializedObject.FindProperty("bulletSpread");
        iDamage = serializedObject.FindProperty("key");
        iDamage = serializedObject.FindProperty("type");
        iDamage = serializedObject.FindProperty("flashlight");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(fSpread);
        base.OnInspectorGUI();
    }
}
