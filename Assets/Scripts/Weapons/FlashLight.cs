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
                ExtDebug.DrawBoxCastBox(center, halfExt, transform.rotation, transform.forward, stats.range, Color.cyan);
                break;
            case FlashLightSO.LightShape.CONIC:
                Gizmos.DrawWireSphere(transform.position, stats.range);
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
            stats.enemiesMask);

        
        
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
        RaycastHit[] hits = Physics.BoxCastAll(center, halfExt, transform.forward, transform.rotation, stats.range);

        
        
        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                hit.collider.GetComponent<IEnemy>()?.TakeVeil(stats.damagesPerTick);
            }
        }

    }
    
    
    

}
