using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StaticFlashLight : MonoBehaviour
{
    [SerializeField] private bool isActive;
    [SerializeField] private Light light;
    [SerializeField] private float tickRate, damagesPerTick;
    [Range(1,90)]
    [SerializeField] private float angle;
    [SerializeField] private float length;
    [SerializeField] private LayerMask enemiesLayer;

    private Coroutine curLoop;

    private void Start()
    {
        light.gameObject.SetActive(isActive);
        if (isActive)
        {
            StartCoroutine(DamageTicksLoop());
        }
    }

    public void ToggleLight()
    {
        isActive = !isActive;
        light.gameObject.SetActive(isActive);

        if (isActive)
        {
            if(curLoop != null) StopCoroutine(curLoop);
            curLoop = StartCoroutine(DamageTicksLoop());
        }
    }

    private void OnDrawGizmos()
    {
        float halfFOV = angle / 2;
        
        Quaternion leftRayRot = Quaternion.AngleAxis( -halfFOV, Vector3.right );
        Quaternion rightRayRot = Quaternion.AngleAxis( halfFOV, Vector3.right );
        Vector3 leftRayDirection = leftRayRot * Vector3.down;
        Vector3 rightRayDirection = rightRayRot * Vector3.down;
        Gizmos.color = Color.green;
        Gizmos.DrawRay( light.transform.position, leftRayDirection * length * 2 );
        Gizmos.DrawRay( light.transform.position, rightRayDirection * length * 2 );

        light.innerSpotAngle = angle;
        light.spotAngle = angle + 5;
        light.range = length * 2;
    }

    private IEnumerator DamageTicksLoop()
    {
        while (isActive)
        {
            //Damage
            ConicDamages();
            
            yield return new WaitForSeconds(tickRate);
        }
    }

    private void ConicDamages()
    {
        RaycastHit[] hits = Physics.SphereCastAll(light.transform.position, length, Vector3.down, length,
            enemiesLayer);
        
        if (hits.Length > 0)
        {
            Debug.Log("touch");
            foreach (var hit in hits)
            {
                float angle = Vector3.Angle(hit.collider.gameObject.transform.position - light.transform.position, Vector3.down);
                if (angle < this.angle)
                {
                    hit.collider.GetComponent<IEnemy>()?.TakeVeil(damagesPerTick);
                }
            }
        }
    }
}
