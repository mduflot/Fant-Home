using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;
using Random = UnityEngine.Random;

public class EquipmentSpawner : Interactible
{
    public int waveToSpawn;
    
    [SerializeField] private List<EquipmentSO> WeaponsToSpawn;
    [SerializeField] private bool containWeapon;
    
    private EquipmentSO curWeapon;

    private void Start()
    {
        GameManager.instance.waveTool.NewWave += CheckIfSpawn;
        containWeapon = false;
    }

    private void CheckIfSpawn(int index)
    {
        if(waveToSpawn == index) Spawn();
    }

    private void Spawn()
    {
        curWeapon = WeaponsToSpawn[Random.Range(0, WeaponsToSpawn.Count)];
        containWeapon = true;
        Debug.Log("Spawn " + curWeapon);
    }
    
    public override void Interact(PlayerInteract play)
    {
        if (containWeapon)
        {
            EquipmentSO newWeapon = null;
            switch (curWeapon.equipType)
            {
                case EquipmentType.GUN :
                    PlayerShooter shooter = play.GetComponent<PlayerShooter>();
                    newWeapon = shooter.GetCurWeapon;
                    shooter.ChangeWeapon((WeaponsSO)curWeapon);
                    break;
                case EquipmentType.LIGHT:
                    FlashLight flashLight = play.GetComponent<FlashLight>();
                    newWeapon = flashLight.GetFlashLight;
                    flashLight.ChangeLight((FlashLightSO)curWeapon);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            curWeapon = newWeapon;
        }
        else Debug.Log("No weapon");
    }
}
