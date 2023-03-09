using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(WaveTool))]
public class WaveToolEditor : UnityEditor.Editor
{
    private VisualElement _root;

    private WaveTool _myWaves;

    [SerializeField] 
    private VisualTreeAsset _visualTree;

    public override VisualElement CreateInspectorGUI()
    {
        _root = new VisualElement();

        _root.Add(_visualTree.Instantiate());

        return _root;
    }

    private void OnEnable()
    {
        _myWaves = (WaveTool)target;
    }
}
