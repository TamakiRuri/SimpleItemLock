
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ItemLockCenter : UdonSharpBehaviour
{
    [SerializeField]private String[] userName;
    [SerializeField]private GameObject[] targetObjects;


    [Header("Action Mode[0]アイテムが消える、[1]アイテムが触れなくなる")]
    [Header("予め[0]オブジェクト[1]コライダーを無効にするとよりセキュアになります")]
    [Header("全ての操作がJoin時に終わるため")]
    [Header("スイッチでオブジェクト(コライダー)を強制的に有効にするとロックが解除されます。")]
    
    
    [Header("Action Mode 0 will make the item disappear, 1 will make the item not touchable")]
    [Header("Deactivating[0]objects[1]colliders before uploading is recommanded for better security")]
    [Header("However, the item will be unlocked if a switch enables the object(collider) directly")]

    [Header(" ")]


    [SerializeField]private int actionMode = 0;
    [SerializeField]private bool allowInstanceOwner = false;

    [Header("Wall Modeでは、動作が逆になります（壁などを一部の人だけがぬけるようにするなど）")]
    [Header("In Wall Mode the function of the script become reversed (for creating walls that can only be go through by whitelisted users).")]
    [Header(" ")]
    [SerializeField]private bool wallMode = false;
    private bool shouldOn = false;
    void Start()
    {
        shouldOn = UserCheck();
        ScriptAction(actionMode, shouldOn);
    }
    private bool UserCheck()
    {
        String localPlayer = Networking.LocalPlayer.displayName;
        if (Networking.LocalPlayer.isInstanceOwner && allowInstanceOwner)
        {
            return true;
        }
        else
        {
            for (int i = 0; i < userName.Length; i++)
            {
                if (localPlayer == userName[i])
                {
                    return true;
                }
            }
        }
        return false;
    }
    private void ScriptAction(int mode, bool targetState){
        switch (mode) {
            case 0:
            foreach(GameObject _gameObject in targetObjects){
                if (!wallMode)_gameObject.SetActive(targetState);
                else _gameObject.SetActive(!targetState);
            }
            break;
            case 1:
            foreach(GameObject _gameObject in targetObjects){
                if (!wallMode)_gameObject.GetComponent<Collider>().enabled = targetState;
                else _gameObject.GetComponent<Collider>().enabled = !targetState;
            }
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
    public String[] ExportUsernames(){
        return userName;
    }
    #endif
}