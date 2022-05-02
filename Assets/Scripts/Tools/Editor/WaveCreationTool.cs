using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class WaveCreationTool : EditorWindow
{
    private string objectName = "";
    private int objectID = 1;
    private int bargeNumber = 1;
    
    
    [MenuItem("Tools/Wave Creation Tool")]
    public static void ShowWindow()
    {
        GetWindow<WaveCreationTool>("Wave Creation Tool");
    }
    private void OnGUI()
    {
        //Basic info on the whole wave
        GUILayout.Label("Wave Editor", EditorStyles.boldLabel);
        objectName = EditorGUILayout.TextField("Wave Base Name", objectName);
        objectID = EditorGUILayout.IntField("Wave ID", objectID);
        
        
        //Infos about the barges
        GUILayout.Space(10);
        GUILayout.Label("Barge", EditorStyles.boldLabel);
        bargeNumber = EditorGUILayout.IntField("Barge Number", bargeNumber);
        for (int i = 1; i >= bargeNumber; i++)
        {
            EditorGUILayout.IntField("Enemy Number",1);
        }
    }
}
