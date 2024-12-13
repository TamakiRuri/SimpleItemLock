#if UNITY_EDITOR && !COMPILER_UDONSHARP
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLockDatabase : MonoBehaviour
{
    [SerializeField] private String[] usernames;
    [Header("必ずプレハブをUnpackしてからお使いください")]

    [Header("Please UNPACK this prefab before using.")]

    [Header(" ")]

    [Header("Modeに関する詳しい説明は、Githubおよび商品ページにあります。")]

    [Header("Specific Inforamtion is on the github and booth page")]

    [Header("Github: https://github.com/TamakiRuri/SimpleItemLock")]

    [Header("Booth: https://saphir.booth.pm/items/6375850")]
    
    [Header(" ")]

    [Header("導入したあと、下のスクリプトにデータが表示されます")]

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
            l_modes[i] = targetObjects[i].mode;
        }
        return l_modes;
    }
    public bool[] ExportAllowOwnerData(){
        bool[] l_allowOwner = new bool[targetObjects.Length];
        for (int i = 0; i < targetObjects.Length; i++){
            l_allowOwner[i] = targetObjects[i].allowInstanceOwner;
        }
        return l_allowOwner;
    }
    public bool[] ExportWallData(){
        bool[] l_wallMode = new bool[targetObjects.Length];
        for (int i = 0; i < targetObjects.Length; i++){
            l_wallMode[i] = targetObjects[i].wallMode;
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
        mode = l_mode;
        allowInstanceOwner = l_allowOwner;
        wallMode = l_wallMode;
    }
    public GameObject targetObject;
    public int mode;
    public bool allowInstanceOwner;

    public bool wallMode;
}
#endif