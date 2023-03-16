using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseGO;
    [SerializeField] private UIManager uiMana;
    [SerializeField] private GameObject firstFocus;
    [SerializeField] private GameObject[] menuButtons;
    [SerializeField] private GameObject optionsGO, optionsFirstFocus;
    private bool active, optionsActive;
    
    public void TogglePause()
    {
        Reset();
        active = !active;
        pauseGO.SetActive(active);
        if (active)
        {
            uiMana.FocusOn(firstFocus);
        }
        Time.timeScale = active?0:1;
    }

    public void ToggleOptions()
    {
        optionsActive = !optionsActive;
        foreach (var button in menuButtons)
        {
            button.SetActive(!optionsActive);
        }
        optionsGO.SetActive(optionsActive);
        uiMana.FocusOn(optionsActive? optionsFirstFocus : firstFocus);
    }

    private void Reset()
    {
        foreach (var buttons in menuButtons)
        {
            buttons.SetActive(true);
        }
        if(optionsActive) ToggleOptions();
    }
}
