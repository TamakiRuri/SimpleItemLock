#if UNITY_EDITOR && !COMPILER_UDONSHARP
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLockDatabase : MonoBehaviour
{
    [SerializeField] private String[] usernames;
    
    [Header("Action Mode[0]アイテムが消える、[1]アイテムが触れなくなる")]
    [Header("予め[0]オブジェクト[1]コライダーを無効にするとよりセキュアになります")]
    [Header("全ての操作がJoin時に終わるため")]
    [Header("スイッチでオブジェクト(コライダー)を強制的に有効にすると、このスクリプトが無効になります。")]
    [Header("Wall Modeでは、動作が逆になります（壁などを一部の人だけがぬけるようにするなど）")]
    [Header("導入が成功したあと、下のスクリプトにデータが表示されます")]
    [Header(" ")]
    [Header("Action Mode 0 will make the item disappear, 1 will make the item not touchable")]
    [Header("Deactivating[0]objects[1]colliders before uploading is recommanded for better security")]
    [Header("However, this script could be overidden by a switch enabling the object(collider) directly")]
    [Header("In Wall Mode the function of the script become reversed (for creating walls that can only be go through by whitelisted users).")]
    [Header("If the data is correctly imported, The script below will show the imported data")]
    [Header(" ")]

    [SerializeField] private ItemLockList[] targetObjects;

    [Header("データ入力が終わりましたら必ずGenerate Dataを押してください。")]
    [Header("Press \"Generate Data\" after imputing your data")]
    [Header(" ")]
    [SerializeField] private GameObject controlCenter;
    

    public void ImportUsernames(String[] l_usernames){
        usernames = l_usernames;
    }
    public void ImportObjectData(GameObject[] l_objects, int[] l_modes, bool[] l_allowOwner, bool[] l_wallModes){
        ItemLockList[] l_list = new ItemLockList[l_objects.Length];
        for (int i = 0; i < l_allowOwner.Length; i++){
            ItemLockList l_entry = new ItemLockList(l_objects[i], l_modes[i], l_allowOwner[i], l_wallModes[i]);
            l_list[i] = l_entry;
        }
        targetObjects = l_list;
    }
    public String[] ExportUserData(){
        String[] l_usernames = new String[usernames.Length];
        for (int i = 0; i < usernames.Length; i++){
            l_usernames[i] = usernames[i];
        }
        return l_usernames;
    }
    public GameObject[] ExportObjectData(){
        GameObject[] l_objects = new GameObject[targetObjects.Length];
        for (int i = 0; i < targetObjects.Length; i++){
            l_objects[i] = targetObjects[i].targetObject;
        }
        return l_objects;
    }
    public int[] ExportModeData(){
        int[] l_modes = new int[targetObjects.Length];
        for (int i = 0; i < targetObjects.Length; i++){
            l_modes[i] = targetObjects[i].targetMode;
        }
        return l_modes;
    }
    public bool[] ExportAllowOwnerData(){
        bool[] l_allowOwner = new bool[targetObjects.Length];
        for (int i = 0; i < targetObjects.Length; i++){
            l_allowOwner[i] = targetObjects[i].targetAllowOwner;
        }
        return l_allowOwner;
    }
    public bool[] ExportWallData(){
        bool[] l_wallMode = new bool[targetObjects.Length];
        for (int i = 0; i < targetObjects.Length; i++){
            l_wallMode[i] = targetObjects[i].targetWallMode;
        }
        return l_wallMode;
    }

    public void GenerateDatatoCenter(){
        if (controlCenter == null) controlCenter = gameObject;
        ItemLockCenterAdvanced centerClass = controlCenter.GetComponent<ItemLockCenterAdvanced>();
        centerClass.ImportUsernames(usernames);
        centerClass.ImportLockData(ExportObjectData(),ExportModeData(),ExportAllowOwnerData(),ExportWallData());
    }
}

[Serializable]public class ItemLockList{
    public ItemLockList(GameObject l_object = null, int l_mode = 0, bool l_allowOwner = false, bool l_wallMode = false){
        targetObject = l_object;
        targetMode = l_mode;
        targetAllowOwner = l_allowOwner;
        targetWallMode = l_wallMode;
    }
    public GameObject targetObject;
    public int targetMode;
    public bool targetAllowOwner;

    public bool targetWallMode;
}
#endif