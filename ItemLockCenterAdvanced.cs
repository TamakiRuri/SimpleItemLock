
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ItemLockCenterAdvanced : UdonSharpBehaviour
{
    [Header("このScriptのデータは自動生成されます")]
    [Header("アイテムを動かしたりすることは、エラーを引き起こす恐れがあります。")]
    [Header("全ての操作がJoin時に終わるため")]
    [Header("スイッチでオブジェクト(コライダー)を強制的に有効にすると、このスクリプトが無効になります。")]

    [Header("The data in this script is auto generated")]
    [Header("Adding or moving objects could potentially break the program")]
    [Header("However, this script could be overidden by a switch enabling the object(collider) directly")]
    [Header(" ")]
    [SerializeField]private String[] userName;
    [SerializeField]private GameObject[] targetObjects;
    [SerializeField]private int[] actionMode;
    [SerializeField]private bool[] allowInstanceOwner;
    [SerializeField]private bool[] wallModes;

    void Start()
    {
        if (wallModes.Length == 0) wallModes = new bool[targetObjects.Length];
        for (int i=0; i< targetObjects.Length; i++){
            ScriptAction(targetObjects[i],actionMode[i], false, wallModes[i]);
            EnableItemorCollider(targetObjects[i], actionMode[i],allowInstanceOwner[i], wallModes[i]);
        }
    }
    private void EnableItemorCollider(GameObject targetObject, int actionMode,bool allowInstanceOwner, bool wallMode){
        String localPlayer = Networking.LocalPlayer.displayName;
        if(Networking.LocalPlayer.isInstanceOwner && allowInstanceOwner){
            ScriptAction(targetObject, actionMode,true, wallMode);
        }
        for (int i =0; i < userName.Length; i++){
            if (localPlayer == userName[i]){
                ScriptAction(targetObject, actionMode,true, wallMode);
            }
        }
    }

    private void ScriptAction(GameObject targetObject, int mode, bool targetState, bool wallMode = false){
        switch (mode) {
            case 0:
                if (!wallMode)targetObject.SetActive(targetState);
                else targetObject.SetActive(!targetState);
            break;
            case 1:
                if (!wallMode)targetObject.GetComponent<Collider>().enabled = targetState;
                else targetObject.GetComponent<Collider>().enabled = !targetState;
            
            break;
            default:
            Debug.LogError("Item Lock: Action Mode Index Out Of Bound.");
            Debug.LogError("Item Lock: Action Modeの入力にエラーを検出しました。");
            break;
        }
        
    }

    
    #if UNITY_EDITOR && !COMPILER_UDONSHARP
    public void ImportUsernames(String[] importedUsernames){
        userName=importedUsernames;
        Debug.Log("Username Imported");
    }

    public void ImportLockData(GameObject[] l_gameObject, int[] l_modes, bool[] l_allowOwner, bool[] l_wallModes){
        targetObjects = l_gameObject;
        actionMode = l_modes;
        allowInstanceOwner = l_allowOwner;
        wallModes = l_wallModes;
    }
    public void ImportTargets(GameObject[] importedGameObjects){
        targetObjects = importedGameObjects;
        Debug.Log("Target GameObjects Imported");
    }
    public void ImportModes(int[] importedModes){
        actionMode = importedModes;
        Debug.Log("Action Modes Imported");
    }
    public void ImportAllowOwner(bool[] importedAllowOwner){
        allowInstanceOwner = importedAllowOwner;
        Debug.Log("Allow Instance Owner Settings Imported");
    }
    public void ImportWallModes(bool[] importedWallModes){
        wallModes = importedWallModes;
        Debug.Log("Wall Mode Settings Imported");
    }
    // Export Functions (not in use)
    public String[] ExportUsernames(){
        return userName;
    }
    
    #endif
    
}