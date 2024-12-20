using System.Collections;
using UnityEditor;
using UnityEngine;
using OleksiiStepanov.UI;

[CustomEditor(typeof(SafeArea))]
public class ApplySafeAreaButton: Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SafeArea myScript = (SafeArea)target;
        if (GUILayout.Button("Apply Safe Area"))
        {
            myScript.Init();
        }
    }
}