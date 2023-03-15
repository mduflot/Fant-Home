using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampFlicker : MonoBehaviour
{
    private int ghostInStack = 0;
    [SerializeField] private LayerMask enemies;
    [SerializeField] private float minFlickerCD = 0.08f, maxFLickerCD = 0.2f;
    private Light lamp;
    private Coroutine runningFlicker;

    private void Awake()
    {
        lamp = transform.parent.GetComponent<Light>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Ghost in range of " + name);
            if (ghostInStack == 0)
            {
                runningFlicker = StartCoroutine(FlickerCoroutine());
            }
            ghostInStack++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            ghostInStack--;
            if (ghostInStack <= 0)
            {
                StopCoroutine(runningFlicker);
                lamp.enabled = true;
            }
        }
    }

    private IEnumerator FlickerCoroutine()
    {
        yield return new WaitForSeconds(Random.Range(minFlickerCD,maxFLickerCD));
        if (Random.Range(0f, 2f) < 1)
        {
            AudioManager.Instance.PlaySFXRandom("Lamp_Flicker1", 0.8f, 1.2f);
        }
        else
        {
            AudioManager.Instance.PlaySFXRandom("Lamp_Flicker2", 0.8f, 1.2f);
        }
        lamp.enabled = !lamp.enabled;
        runningFlicker = StartCoroutine(FlickerCoroutine());
    }
}
