using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class MessageDisplayer : MonoBehaviour
{
    public enum TextHeight
    {
        HEADER,
        SUBTITLE
    }
    [SerializeField] private TMP_Text headerText, subtitleText;
    private Dictionary<TextHeight, TMP_Text> TextField = new Dictionary<TextHeight, TMP_Text>();

    private void Awake()
    {
        for (int i = 0; i < Enum.GetValues(typeof(TextHeight)).Length; i++)
        {
            TextField[TextHeight.HEADER] = headerText;
            TextField[TextHeight.SUBTITLE] = subtitleText;
        }
    }

    public void DisplayText(string message,TextHeight textHeight, float duration = 3)
    {
        TextField[textHeight].text = message;
        TextField[textHeight].gameObject.SetActive(true);
        StartCoroutine(WaitBeforeDisable(textHeight, duration));
    }

    public void DisableText(TextHeight textHeight)
    {
        TextField[textHeight].gameObject.SetActive(false);
    }

    public void DisableText()
    {
        foreach (var field in TextField)
        {
            field.Value.gameObject.SetActive(false);
        }
    }

    private IEnumerator WaitBeforeDisable(TextHeight field, float time)
    {
        yield return new WaitForSeconds(time);
        DisableText(field);
        
    }
}
