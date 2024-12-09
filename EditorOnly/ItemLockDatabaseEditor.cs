
#if UNITY_EDITOR && !COMPILER_UDONSHARP

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(ItemLockDatabase))]
public class ItemLockDatabaseEditor : Editor
{
    GameObject targetLock = null;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ItemLockDatabase managedScript = (ItemLockDatabase)target;
        if (GUILayout.Button("Generate Data")){
            try {
            managedScript.GenerateDatatoCenter();
            }
            catch (Exception e){
                throw e;
            }
            Debug.Log("Data Generated Successfully");
        }

        EditorGUILayout.LabelField("");
        EditorGUILayout.LabelField("Copy Username");
        targetLock = (GameObject)EditorGUILayout.ObjectField("Target Lock", targetLock, typeof(GameObject), true);
        
        if (GUILayout.Button("Copy Username")){
            try {
                if(targetLock.GetComponent<ItemLockDatabase>()!= null){
                    targetLock.GetComponent<ItemLockDatabase>().ImportUsernames(managedScript.ExportUserData());
                    targetLock.GetComponent<ItemLockDatabase>().GenerateDatatoCenter();
                }
                else if (targetLock.GetComponent<ItemLockCenter>()!= null){
                    targetLock.GetComponent<ItemLockCenter>().ImportUsernames(managedScript.ExportUserData());
                }
                else if (targetLock.GetComponent<ItemLockUsername>()!= null)
                targetLock.GetComponent<ItemLockUsername>().ImportUsernames(managedScript.ExportUserData());
                else throw new ArgumentException("ItemLockDatabase: Cannot find ItemLock script in target lock.");
            }
            catch (Exception e){
                throw e;
            }
            Debug.Log("Data Copied Successfully");
        }
    }
    
}
#endif
