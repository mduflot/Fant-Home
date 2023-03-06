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
        // Vector3 vp = Camera.main.WorldToViewportPoint(transform.position);
        // Vector3 bottomPoint = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, vp.z));
        //
        // if (vp.y < 0f)
        // {
        //     transform.position = new Vector3(transform.position.x, transform.position.y, bottomPoint.z);
        //     return;  // Short circuit other imput
        // }
        
        transform.Translate(Time.deltaTime * moveSpeed * new Vector3(moveVal.x, 0, moveVal.y));
    }

}
