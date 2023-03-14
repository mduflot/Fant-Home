using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSoundTrigger : MonoBehaviour
{
    public void PlayClic()
    {
        AudioManager.Instance.PlaySFXRandom("Menu_Clic", 0.95f, 1.05f);
    }
    public void PlayHover()
    {
        AudioManager.Instance.PlaySFXRandom("Menu_Hover", 0.95f, 1.05f);
    }
}
