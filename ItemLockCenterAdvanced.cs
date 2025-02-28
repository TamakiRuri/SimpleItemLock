
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ItemLockCenterAdvanced : LibItemLock
{
    [Header("このScriptのデータは自動生成されます")]
    [Header("アイテムを動かしたりすることは、エラーを引き起こす恐れがあります。")]

    [Header("The data in this script is auto generated")]
    [Header("Adding or moving objects could potentially break the program")]
    [Header(" ")]
    [SerializeField] protected String[] usernames;
    [SerializeField] protected GameObject[] targetObjects;
    [SerializeField] protected int[] actionMode;
    [SerializeField] protected bool[] allowInstanceOwner;
    [SerializeField] protected bool[] wallModes;

    // allow InstanceOwner is different for each object this only applies to list members
    protected bool userInList = false;

    protected void Start()
    {
        userInList = UserListCheck();
        if (wallModes.Length == 0) wallModes = new bool[targetObjects.Length];
        for (int i = 0; i < targetObjects.Length; i++)
        {
            // This shouldOn is the same as Lock Center and basic
            bool shouldOn = (Networking.LocalPlayer.isInstanceOwner && allowInstanceOwner[i]) || userInList;
            // (shouldOn && !wallModes[i]) || (!shouldOn && wallModes[i])
            // in non wall mode, output == shouldOn
            // in wall mode, output == !shouldOn
            bool targetState = (shouldOn && !wallModes[i]) || (!shouldOn && wallModes[i]);
            ScriptAction(targetObjects[i], actionMode[i], targetState);
        }
    }
    // UserCheck. Different funcion name to prevent misunderstandings. THIS DOES NOT CHECK INSTANCE OWNER.
    protected bool UserListCheck()
    {
        String localPlayer = Networking.LocalPlayer.displayName;
        for (int i = 0; i < usernames.Length; i++)
        {
            if (localPlayer == usernames[i])
            {
                return true;
            }
        }
        return false;
    }

#if UNITY_EDITOR && !COMPILER_UDONSHARP
    public void ImportUsernames(String[] importedUsernames)
    {
        usernames = importedUsernames;
        Debug.Log("Username Imported");
    }

    public void ImportLockData(GameObject[] l_gameObject, int[] l_modes, bool[] l_allowOwner, bool[] l_wallModes)
    {
        targetObjects = l_gameObject;
        actionMode = l_modes;
        allowInstanceOwner = l_allowOwner;
        wallModes = l_wallModes;
        Debug.Log("Item Lock Advanced: Data Imported");
    }
    public void ImportTargets(GameObject[] importedGameObjects)
    {
        targetObjects = importedGameObjects;
        Debug.Log("Item Lock Advanced: Target GameObjects Imported");
    }
    public void ImportModes(int[] importedModes)
    {
        actionMode = importedModes;
        Debug.Log("Item Lock Advanced: Action Modes Imported");
    }
    public void ImportAllowOwner(bool[] importedAllowOwner)
    {
        allowInstanceOwner = importedAllowOwner;
        Debug.Log("Item Lock Advanced: Allow Instance Owner Settings Imported");
    }
    public void ImportWallModes(bool[] importedWallModes)
    {
        wallModes = importedWallModes;
        Debug.Log("Item Lock Advanced: Wall Mode Settings Imported");
    }
    // Export Functions (not in use)
    public String[] ExportUsernames()
    {
        return usernames;
    }

#endif

}