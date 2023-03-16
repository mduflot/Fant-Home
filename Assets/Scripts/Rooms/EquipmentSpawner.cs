using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entities;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

public class EquipmentSpawner : Interactible
{
    public int waveToSpawn;
    
    [SerializeField] private List<EquipmentSO> WeaponsToSpawn;
    
    [Header("Debug")]
    [SerializeField] private bool containWeapon;
    [SerializeField] private EquipmentSO curWeapon;
    [SerializeField] private Material emptyMat;
    [SerializeField] private Material containWeaponMat;
    [SerializeField] private MeshRenderer mesh;
    [SerializeField] private Collider col;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private TMP_Text infoText, nameText;
    [SerializeField] private Image weaponLogo;
    
    private void Start()
    {
        GameManager.instance.waveTool.NewWave += CheckIfSpawn;
        containWeapon = false;
        mesh.material = emptyMat;
        mesh.gameObject.SetActive(false);
        col.enabled = false;
    }

    private void CheckIfSpawn(int index)
    {
        if(waveToSpawn == index) Spawn();
    }

    private void Spawn()
    {
        curWeapon = WeaponsToSpawn[Random.Range(0, WeaponsToSpawn.Count)];
        containWeapon = true;
        mesh.gameObject.SetActive(true);
        col.enabled = true;
        mesh.material = containWeaponMat;
        Debug.Log("Spawn " + curWeapon);
        UpdateInfos();
    }
    
    public override void Interact(PlayerInteract play)
    {
        if (containWeapon)
        {
            EquipmentSO newWeapon = null;
            Player player = play.GetComponent<Player>();
            switch (curWeapon.equipType)
            {
                case EquipmentType.GUN :
                    PlayerShooter shooter = player.playerShoot;
                    newWeapon = shooter.GetCurWeapon;
                    shooter.ChangeWeapon((WeaponsSO)curWeapon);
                    Debug.Log("Player took " + curWeapon + " and dropped his " + newWeapon);
                    break;
                case EquipmentType.LIGHT:
                    FlashLight flashLight = player.flashLight;
                    newWeapon = flashLight.GetFlashLight;
                    flashLight.ChangeLight((FlashLightSO)curWeapon);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            curWeapon = newWeapon;
            UpdateInfos();
        }
        else Debug.Log("No weapon");
    }

    public override void PlayerEnter(PlayerInteract player)
    {
        infoPanel.SetActive(true);
        if(containWeapon) UpdateInfos();
    }

    private void UpdateInfos()
    {
        infoText.text = curWeapon.desc;
        nameText.text = curWeapon.weaponName;
        weaponLogo.sprite = curWeapon.icon;
    }

    public override void PlayerExit(PlayerInteract player)
    {
        if(curPlayers.Count == 0) infoPanel.SetActive(false);
    }
}
