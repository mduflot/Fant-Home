using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    
    private Vector2 _moveVal;
    private Vector2 _rotateVal;
    
    public float moveSpeed;

    public bool oneStick;
    public bool triggerFire;

    void OnMove(InputValue value)
    {
        _moveVal = value.Get<Vector2>();
    }

    void OnRotate(InputValue value)
    {
        _rotateVal = value.Get<Vector2>();
    }

    void OnFire(InputValue value)
    {
        if (triggerFire)
        {
            Fire();
        }
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

        if (!triggerFire) CheckFire();
        
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void CheckFire()
    {
        if (rotateVal.x > 0.99f || rotateVal.x < -0.99f || rotateVal.y > 0.99f || rotateVal.y < -0.99f)
        {
            Fire();
        }
    }

    private void Fire()
    {
        Debug.Log("Fire");
    }

    private void DoMove()
    {
        if (_moveVal == Vector2.zero) return;
        transform.position += (Time.deltaTime * moveSpeed * new Vector3( _moveVal.x, 0, _moveVal.y));
    }

    private void DoRotate()
    {
        if (_rotateVal == Vector2.zero) return;
        transform.rotation = Quaternion.LookRotation(new Vector3(_rotateVal.x, 0, _rotateVal.y));
    }

    private void DoMoveAndRotate()
    {
        if (_moveVal == Vector2.zero) return;
        transform.rotation = Quaternion.LookRotation(new Vector3(_moveVal.x, 0, _moveVal.y));
        transform.Translate(Time.deltaTime * moveSpeed * Vector3.forward);
    }
}
