
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

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

    [SerializeField] protected int actionMode = 0;
    [SerializeField] protected bool allowInstanceOwner = false;
    [SerializeField] protected bool wallMode = false;
    protected bool shouldOn = false;
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
    protected bool UserCheck()
    {
        String localPlayer = Networking.LocalPlayer.displayName;
        if (Networking.LocalPlayer.isInstanceOwner && allowInstanceOwner)
        {
            return true;
        }
        else
        {
            for (int i = 0; i < usernames.Length; i++)
            {
                if (localPlayer == usernames[i])
                {
                    return true;
                }
            }
        }
        return false;
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