
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class ItemLockCenterAdvanced : LibItemLock
{
    [Header("このScriptのデータは自動生成されます")]
    [Header("アイテムを動かしたりすることは、エラーを引き起こす恐れがあります。")]

    [Header("The data in this script is auto generated")]
    [Header("Adding or moving objects could potentially break the program")]
    [Header(" ")]
    [SerializeField] private String masterPassword;
    [SerializeField] protected String[] usernames;
    [SerializeField] protected GameObject[] targetObjects;
    [SerializeField] protected int[] actionMode;
    [SerializeField] protected bool[] allowInstanceOwner;
    [SerializeField] protected bool fallbackToMaster;
    [SerializeField] protected bool[] wallModes;
    [SerializeField] private String password;
    [SerializeField] private int timeOutCount = 5;
    private int wrongInputs = 0;
    // For safety, if the master password is wrong for a single time the script will be locked
    private bool lockFeature = false;

    // allow InstanceOwner is different for each object this only applies to list members
    protected bool userInList = false;
    [UdonSynced]private String createdPassword;
    [UdonSynced]private String savedMaster;

    protected void Start()
    {
        if (Networking.LocalPlayer.isMaster && fallbackToMaster)
        {
            savedMaster = Networking.LocalPlayer.displayName;
            RequestSerialization();
        }
        userInList = UserListCheck();
        if (wallModes.Length == 0) wallModes = new bool[targetObjects.Length];
        for (int i = 0; i < targetObjects.Length; i++)
        {
            // This shouldOn is the same as Lock Center and basic
            bool shouldOn = ((Networking.LocalPlayer.isInstanceOwner || (fallbackToMaster && savedMaster == Networking.LocalPlayer.displayName)) && allowInstanceOwner[i]) || userInList;
            // (shouldOn && !wallModes[i]) || (!shouldOn && wallModes[i])
            // in non wall mode, output == shouldOn
            // in wall mode, output == !shouldOn
            bool targetState = (shouldOn && !wallModes[i]) || (!shouldOn && wallModes[i]);
            ScriptAction(targetObjects[i], actionMode[i], targetState);
        }
    }
    // UserCheck. Different funcion name to prevent misunderstandings. THIS DOES NOT CHECK INSTANCE OWNER.
    private bool UserListCheck()
    {
        String localPlayer = Networking.LocalPlayer.displayName;
        if (Array.IndexOf(usernames, localPlayer)!= -1) return true;
        else
        return false;
    }
        // -1: Not Enabled 0: Unlock, 1: Wrong Password, 2: Already Unlocked, 3: Input Blocked
    public int PasswordCheck(String l_password){
        if (password == ""&& createdPassword == "") {return -1;}
        if (userInList) return 2;
        if (wrongInputs < timeOutCount || timeOutCount == 0){
            if (l_password == password || (createdPassword != "" && l_password == createdPassword)){
                userInList = true;
                for(int i = 0; i < targetObjects.Length; i++)
                {
                    ScriptAction(targetObjects[i], actionMode[i], !wallModes[i]);
                }
                Debug.Log("Simple Item Lock Advanced: Unlocked.\n ロック解除しました。");
                return 0;
            }
            else {
                wrongInputs++;
                return 1;
            }
        }
        else return 3;
    }
    public void CreatePasswordMP(String l_password, String l_masterPassword){
        if (lockFeature == true) return;
        if (l_masterPassword != masterPassword){
            lockFeature = true;
            return;
        }
        createdPassword = l_password;
        if (!Networking.IsOwner(Networking.LocalPlayer, gameObject)){
			Networking.SetOwner(Networking.LocalPlayer, gameObject);
		}
        RequestSerialization();
    }
    public void ImportUsernamesMP(String[] l_usernames, String l_masterPassword){
        if (lockFeature == true) return;
        if (l_masterPassword != masterPassword){
            lockFeature = true;
            return;
        }
        usernames = MergetoArray(usernames, l_usernames);
    }

#if UNITY_EDITOR && !COMPILER_UDONSHARP
    public void ImportUsernames(String[] importedUsernames)
    {
        usernames = importedUsernames;
        Debug.Log("Username Imported");
    }

    public void ImportLockData(GameObject[] l_gameObject, int[] l_modes, bool[] l_allowOwner, bool[] l_wallModes, bool l_fallback)
    {
        targetObjects = l_gameObject;
        actionMode = l_modes;
        allowInstanceOwner = l_allowOwner;
        fallbackToMaster = l_fallback;
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
    public void ImportFallbackToMaster(bool importedFallbackToMaster)
    {
        fallbackToMaster = importedFallbackToMaster;
        Debug.Log("Item Lock Advanced: Fall back to master Settings Imported");
    }
    public void ImportWallModes(bool[] importedWallModes)
    {
        wallModes = importedWallModes;
        Debug.Log("Item Lock Advanced: Wall Mode Settings Imported");
    }
    public void ImportPasswordData(String l_password, int l_timeOutCount){
        password = l_password;
        timeOutCount = l_timeOutCount;
    }
    public void ImportMasterPassword(String l_masterPassword){
        masterPassword = l_masterPassword;
    }
    // Export Functions (not in use)
    public String[] ExportUsernames()
    {
        return usernames;
    }

#endif

}