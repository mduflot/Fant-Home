using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;
using Random = UnityEngine.Random;

public class EquipmentSpawner : Interactible
{
    public int waveToSpawn;
    
    [SerializeField]
    private List<EquipmentSO> WeaponsToSpawn;
    
    private EquipmentSO weaponToSpawn;

    private void Start()
    {
        GameManager.instance.waveTool.NewWave += CheckIfSpawn;
    }

    private void CheckIfSpawn(int index)
    {
        if(waveToSpawn == index) Spawn();
    }

    private void Spawn()
    {
        weaponToSpawn = WeaponsToSpawn[Random.Range(0, WeaponsToSpawn.Count)];
        Debug.Log("Spawn " + weaponToSpawn);
    }
    
    public override void Interact(PlayerInteract play)
    {
        switch (weaponToSpawn.equipType)
        {
            case EquipmentType.GUN :
                
                play.GetComponent<PlayerShooter>().ChangeWeapon((WeaponsSO)weaponToSpawn);
                break;
            case EquipmentType.LIGHT:
                play.GetComponent<FlashLight>().ChangeLight((FlashLightSO)weaponToSpawn);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
