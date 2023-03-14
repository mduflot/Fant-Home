using Entities;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Room curRoom;
    [SerializeField] public PlayerShooter playerShoot;
    [SerializeField] public FlashLight flashLight;
    
    private void Start()
    {
        curRoom?.PlayerEnter(this);
    }
}
