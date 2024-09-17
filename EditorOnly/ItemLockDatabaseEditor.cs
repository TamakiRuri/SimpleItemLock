
#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;



[CustomEditor(typeof(ItemLockDatabase))]
public class ItemLockDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ItemLockDatabase managedScript = (ItemLockDatabase)target;
        if (GUILayout.Button("Generate Data")){
            try {
            managedScript.generateDatatoCenter();
            }
            catch (Exception e){
                throw e;
            }
            Debug.Log("Data Generated Successfully");
        }
    }
    
}
#endif
