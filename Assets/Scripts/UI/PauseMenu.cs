using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject PauseGO;
    [SerializeField] private UIManager uiMana;
    [SerializeField] private GameObject firstFocus;
    private bool active;
    
    public void TogglePause()
    {
        active = !active;
        PauseGO.SetActive(active);
        if (active)
        {
            uiMana.FocusOn(firstFocus);
        }

        Time.timeScale = active?0:1;
    }
}
