using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReanimateInteraction : Interactible
{
    [SerializeField] private Slider slider;
    [SerializeField] private PlayerHealth playerHealth;
    
    private float progress;
    private PlayerInteract currentReviver;
    private Coroutine curLoop;
    
    
    public override void Interact(PlayerInteract player)
    {
        if (currentReviver != null) return;
        
        currentReviver = player;
        curLoop = StartCoroutine(ReviveTime());
    }

    public override void PlayerExit(PlayerInteract player)
    {
        if (currentReviver != null)
        {
            if(player == currentReviver) StopRevive();
        }
    }

    private void StopRevive()
    {
        if(curLoop != null)StopCoroutine(curLoop);
        progress = 0;
        slider.value = 0;
    }

    IEnumerator ReviveTime()
    {
        progress = 0;
        while (progress < 2)
        {
            yield return new WaitForEndOfFrame();
            progress += Time.deltaTime;
            slider.value = progress;
        }
        
        slider.value = 0;
        currentReviver = null;
        playerHealth.GetUp();
    }
}
