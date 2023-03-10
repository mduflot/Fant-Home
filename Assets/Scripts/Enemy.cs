using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Enemy
{
    
    protected int health { get; }
    protected string name { get; }
    protected int damage { get; }
    
    protected int speed { get; }

    public void Attack();
    public void TakeDamage();
}
