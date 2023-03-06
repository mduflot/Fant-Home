using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    
    public Vector2 moveVal;
    public Vector2 rotateVal;
    
    public float moveSpeed;
    public float rotateSpeed;

    public bool oneStick;

    void OnMove(InputValue value)
    {
        moveVal = value.Get<Vector2>();
    }

    void OnRotate(InputValue value)
    {
        rotateVal = value.Get<Vector2>();
    }

    void Update()
    {
        if (oneStick)
        {
            DoMoveAndRotate();
        }
        else
        {
            DoRotate();
            DoMove();
        }
    }

    private void DoMove()
    {
        if (moveVal == Vector2.zero) return;
        transform.position += (Time.deltaTime * moveSpeed * new Vector3( moveVal.x, 0, moveVal.y));
    }

    private void DoRotate()
    {
        if (rotateVal == Vector2.zero) return;
        transform.rotation = Quaternion.LookRotation(new Vector3(rotateVal.x, 0, rotateVal.y));
    }

    private void DoMoveAndRotate()
    {
        if (moveVal == Vector2.zero) return;
        transform.rotation = Quaternion.LookRotation(new Vector3(moveVal.x, 0, moveVal.y));
        transform.Translate(Time.deltaTime * moveSpeed * Vector3.forward);
    }
}
