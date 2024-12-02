
#if UNITY_EDITOR && !COMPILER_UDONSHARP

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using VRC.Udon;

[CustomEditor(typeof(ItemLockDatabase))]
public class ItemLockDatabaseEditor : Editor
{
    GameObject otherLock = null;
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

        EditorGUILayout.LabelField("");
        EditorGUILayout.LabelField("Copy Usernames Data");
        otherLock = (GameObject)EditorGUILayout.ObjectField("Target Lock", otherLock, typeof(GameObject), true);
        
        if (GUILayout.Button("Copy Usernames Data")){
            try {
                if(otherLock.GetComponent<ItemLockDatabase>()!= null){
                    otherLock.GetComponent<ItemLockDatabase>().importUsernames(((ItemLockDatabase)target).exportUserData());
                    otherLock.GetComponent<ItemLockDatabase>().generateDatatoCenter();
                }
                else {
                    ((UdonBehaviour)otherLock.GetComponent<UdonBehaviour>()).SendMessage("importUsernames", ((ItemLockDatabase)target).exportUserData());
                }
            }
            catch (Exception e){
                throw e;
            }
            Debug.Log("Data Copied Successfully");
        }
    }
    
}
#endif
