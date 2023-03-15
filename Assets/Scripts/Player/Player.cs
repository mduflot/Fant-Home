using Entities;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Room curRoom;
    [SerializeField] public PlayerShooter playerShoot;
    [SerializeField] public FlashLight flashLight;
    public PlayerUI playerUI;
    
    private void Start()
    {
        curRoom?.PlayerEnter(this);
    }
}
