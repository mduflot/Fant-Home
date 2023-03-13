using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class FlashLight : MonoBehaviour
{
    public FlashLightSO stats;
    
    [SerializeField] private GameObject flashLightGO;
    [SerializeField] private bool isActive;
    [SerializeField] private LayerMask obstaclesMask, enemiesMask;

    private Coroutine curLoop;
    private void Start()
    {
        flashLightGO.SetActive(isActive);
    }

    private void OnDrawGizmos()
    {
        switch (stats.shape)
        {
            case FlashLightSO.LightShape.BOX:
                Vector3 center = transform.position + (transform.forward * stats.range / 2);
                Vector3 halfExt = new Vector3(stats.width, stats.width, stats.range / 2);
                ExtDebug.DrawBoxCastBox(center, halfExt, transform.rotation, transform.forward, stats.range, Color.green);
                break;
            case FlashLightSO.LightShape.CONIC:
                float halfFOV = stats.angle / 2;
                
                Quaternion leftRayRot = Quaternion.AngleAxis( -halfFOV, Vector3.up );
                Quaternion rightRayRot = Quaternion.AngleAxis( halfFOV, Vector3.up );
                Vector3 leftRayDirection = leftRayRot * transform.forward;
                Vector3 rightRayDirection = rightRayRot * transform.forward;
                Gizmos.color = Color.green;
                Gizmos.DrawRay( transform.position, leftRayDirection * stats.range );
                Gizmos.DrawRay( transform.position, rightRayDirection * stats.range );
                
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
    }

    private void OnLight()
    {
        isActive = !isActive;
        flashLightGO.SetActive(isActive);
        
        if(curLoop != null) StopCoroutine(curLoop);
        curLoop = StartCoroutine(DamageTicksLoop());
    }

    private IEnumerator DamageTicksLoop()
    {
        while (isActive)
        {
            //Damage
            switch (stats.shape)
            {
                case FlashLightSO.LightShape.BOX:
                    BoxDamages();
                    break;
                case FlashLightSO.LightShape.CONIC:
                    ConicDamages();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            yield return new WaitForSeconds(stats.tickRate);
        }
    }

    private void ConicDamages()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, stats.range, transform.forward, stats.range,
            enemiesMask);

        
        
        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                float angle = Vector3.Angle(hit.collider.gameObject.transform.position - transform.position, transform.forward);
                if (angle < stats.angle)
                {
                    hit.collider.GetComponent<IEnemy>()?.TakeVeil(stats.damagesPerTick);
                }
            }
        }
    }

    private void BoxDamages()
    {
        Vector3 center = transform.position + (transform.forward * stats.range / 2);
        Vector3 halfExt = new Vector3(stats.width, stats.width, stats.range / 2);
        RaycastHit[] hits = Physics.BoxCastAll(center, halfExt, transform.forward, transform.rotation, stats.range, enemiesMask);
        
        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                hit.collider.GetComponent<IEnemy>()?.TakeVeil(stats.damagesPerTick);
            }
        }
    }

    private bool IsVisible(Transform trans)
    {
        RaycastHit hit = new RaycastHit();
        if (Physics.Linecast(transform.position, trans.position, out hit, obstaclesMask))
        {
            return hit.collider;
        }

        return true;
    }

    public void ChangeLight(FlashLightSO newLight)
    {
        stats = newLight;
    }
    

}
