
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ItemLockUsername : UdonSharpBehaviour
{
    [Header("何も入れない場合、現在のオブジェクトが設定されます。")]
    [Header("If no object is here, current object will be automatically set up")]
    [Header(" ")]
    [SerializeField] private GameObject targetObject;
    [SerializeField] private String[] userName;

    [Header("Action Mode[0]アイテムが消える、[1]アイテムが触れなくなる")]
    [Header("[1]予めコライダーを無効にするとよりセキュアになります")]
    [Header("スイッチでオブジェクト(コライダー)を強制的に有効にすると、このスクリプトが無効になります。")]


    [Header("Action Mode 0 will make the item disappear, 1 will make the item not touchable")]
    [Header("[1]Deactivating colliders before uploading is recommanded for better security")]
    [Header("However, this script could be overidden by a switch enabling the object(collider) directly")]
    [Header(" ")]


    [SerializeField] private int actionMode = 0;

    [SerializeField] private bool allowInstanceOwner = false;

    [Header("Wall Modeでは、動作が逆になります（壁などを一部の人だけがぬけるようにするなど）")]
    [Header("In Wall Mode the function of the script become reversed (for creating walls that can only be go through by whitelisted users).")]
    [Header(" ")]

    [SerializeField] private bool wallMode = false;

    private bool shouldOn = false;
    private Collider targetCollider;

    void Start()
    {
        if (targetObject == null) targetObject = gameObject;
        shouldOn = userCheck();
        if (actionMode == 1) {
            targetCollider = targetObject.GetComponent<Collider>();
            if (targetCollider == null) {
                Debug.LogError("Item Lock: This object is in Action Mode 1 but the Collider can't be detected");
                Debug.LogError("Item Lock: このオブジェクトの動作モードがAction Mode 1ですがコライダーを取得できません");
                return;
            }
        }
        enableItemorCollider();
    }

    private bool userCheck()
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

    private void ObjectMode()
    {
        if (!wallMode) targetObject.SetActive(false);
        else targetObject.SetActive(true);
        if (shouldOn)
        {
            if (!wallMode) targetObject.SetActive(true);
            else targetObject.SetActive(false);
        }
    }
    private void ColliderMode()
    {
        if (!wallMode) targetObject.GetComponent<Collider>().enabled = false;
        else targetObject.GetComponent<Collider>().enabled = true;

        if (shouldOn)
        {
            if (!wallMode) targetObject.GetComponent<Collider>().enabled = true;
            else targetObject.GetComponent<Collider>().enabled = false;
        }
    }

    private void enableItemorCollider()
    {
        switch (actionMode)
        {
            case 0:
                ObjectMode();
                break;
            case 1:
                ColliderMode();
                break;
            default:
                Debug.LogError("Item Lock: Action Mode Index Out Of Bound.");
                Debug.LogError("Item Lock: Action Modeを0か1にしてください。");
                break;
        }
    }
    #if UNITY_EDITOR && !COMPILER_UDONSHARP
    public void importUsernames(String[] importedUsernames){
        userName=importedUsernames;
        Debug.Log("Username Imported");
    }
    #endif

}
