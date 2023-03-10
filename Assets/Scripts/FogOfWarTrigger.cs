using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FogOfWarTrigger : MonoBehaviour
{
    public DecalProjector projector;
    
    private Coroutine curFadeTransition;

    // private void OnDrawGizmosSelected()
    // {
    //     if(snap) {transform.position = (Vector3)new Vector3Int((int)transform.position.x, (int)transform.position.y,
    //         (int)transform.position.z);}
    //     
    //     if (projector == null) return;
    //     projector.size = new Vector3(size.x, size.z, size.y);
    //
    //     transform.localScale = Vector3.one;
    //     #if UNITY_EDITOR
    //     //UnityEditor.EditorUtility.SetDirty(gameObject);
    //     #endif
    // }

    public void DisplayRoom()
    {
        if(curFadeTransition != null) StopCoroutine(curFadeTransition);
        curFadeTransition = StartCoroutine(FadeTransition(false));
    }

    public void HideRoom()
    {
        if(curFadeTransition != null)StopCoroutine(curFadeTransition);
        curFadeTransition = StartCoroutine(FadeTransition(true));
    }

    private IEnumerator FadeTransition(bool hide)
    {
        while ((hide ? projector.fadeFactor < 0.7f : projector.fadeFactor > 0))
        {
            projector.fadeFactor = Mathf.Clamp(projector.fadeFactor + Time.deltaTime * (hide ? 1 : -1),0f,1f);
            yield return new WaitForEndOfFrame();
        }
        curFadeTransition = null;
    }
}
