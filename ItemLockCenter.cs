
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class ItemLockCenter : LibItemLock
{
    [SerializeField] protected String[] usernames;

    [Header("何も入れない場合、現在のオブジェクトが設定されます。")]
    [Header("If no object is attached, current object will be automatically set up")]
    [Header(" ")]
    [SerializeField] protected GameObject[] targetObjects;


    [Header("Modeに関する詳しい説明は、Githubおよび商品ページにあります。")]

    [Header("Specific Inforamtion is on the github and booth page")]

    [Header("Github: https://github.com/TamakiRuri/SimpleItemLock")]

    [Header("Booth: https://saphir.booth.pm/items/6375850")]

    [Header(" ")]

    [Header("Master Passwordスクリプト連携用で、ワールド内では通常のパスワードとしてご利用できません")]
    [Header("For script interactions. Does NOT work for world users.")]

    [SerializeField] private String masterPassword="StudioSaphir";
    [SerializeField] protected int actionMode = 0;
    [SerializeField] protected bool allowInstanceOwner = false;
    [SerializeField] protected bool wallMode = false;
    protected bool shouldOn = false;
    [Header("パスワード: 数字のみ")]
    [Header("Password: Numbers only")]
    [SerializeField] private String password;
    [Header("タイムアウトが発生するまでパスワードを連続に間違える回数。0 では無効になります。")]
    [Header("タイムアウトになった場合、Rejoinしなければパスワード入力が処理されなくなります。")]
    [Header("Amount of wrong passwords before the player to get timed out. 0 for disable.")]
    [Header("If a player is timed out, later password inputs are not going to be processed unless the player rejoins.")]
    [SerializeField] private int timeOutCount = 5;
    private int wrongInputs = 0;
    // For safety, if the master password is wrong for a single time the script will be locked
    private bool lockFeature = false;
    [UdonSynced]private String createdPassword;
    protected void Start()
    {
        if (targetObjects.Length == 0){
            targetObjects = new GameObject[1];
            targetObjects[0] = gameObject;
        }
        shouldOn = UserCheck();
        // (shouldOn && !wallMode) || (!shouldOn && wallMode)
        // in non wall mode, output == shouldOn
        // in wall mode, output == !shouldOn
        foreach (GameObject targetObject in targetObjects)
        {
            ScriptAction(targetObject, actionMode, (shouldOn && !wallMode) || (!shouldOn && wallMode));
        }

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
            if (Array.IndexOf(usernames, localPlayer)!= -1) return true;
        }
        return false;
    }
    // -1: Not Enabled 0: Unlock, 1: Wrong Password, 2: Already Unlocked, 3: Input Blocked
    public int PasswordCheck(String l_password){
        if (password == "" && createdPassword == "") return -1;
        if (shouldOn) return 2;
        if ( wrongInputs < timeOutCount || timeOutCount == 0){
            if (l_password == password || (createdPassword != "" && l_password == createdPassword)){
                shouldOn = true;
                foreach (GameObject targetObject in targetObjects)
                {
                    ScriptAction(targetObject, actionMode, !wallMode);
                }
                Debug.Log("Simple Item Lock Advanced: Unlocked.");
                return 0;
            }
            else{
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
        RequestSerialization();
    }
    public void ImportUsernamesMP(String[] l_usernames, String l_password){
        if (lockFeature == true) return;
        if (l_password != masterPassword){
            lockFeature = true;
            return;
        }
        usernames = MergetoArray(usernames, l_usernames);
    }
#if UNITY_EDITOR && !COMPILER_UDONSHARP
    public void ImportUsernames(String[] importedUsernames)
    {
        usernames = importedUsernames;
        Debug.Log("Item Lock Center: Username Imported");
    }
    public String[] ExportUsernames()
    {
        return usernames;
    }
#endif
}