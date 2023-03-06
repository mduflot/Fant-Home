using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    
    public Vector2 moveVal;
    public float moveSpeed;
    
    void OnMove(InputValue value)
    {
        moveVal = value.Get<Vector2>();
    }

    void Update()
    {
        transform.Translate(Time.deltaTime * moveSpeed * new Vector3(moveVal.x, 0, moveVal.y));
    }

}
