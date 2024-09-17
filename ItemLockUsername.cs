
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
    [SerializeField]private GameObject targetObject;
    [SerializeField]private String[] userName;
    
    [Header("Action Mode[0]アイテムが消える、[1]アイテムが触れなくなる")]
    [Header("[1]予めコライダーを無効にするとよりセキュアになります")]
    [Header("全ての操作がJoin時に終わるため")]
    [Header("スイッチでオブジェクト(コライダー)を強制的に有効にすると、このスクリプトが無効になります。")]
    
    
    [Header("Action Mode 0 will make the item disappear, 1 will make the item not touchable")]
    [Header("[1]Deactivating colliders before uploading is recommanded for better security")]
    [Header("However, this script could be overidden by a switch enabling the object(collider) directly")]
    [Header(" ")]


    [SerializeField]private int actionMode = 0;
    
    [SerializeField]private bool allowInstanceOwner = false;

    [SerializeField]private bool wallMode = false;

    void Start()
    {
        if (targetObject == null) targetObject=gameObject;
        enableItemorCollider(actionMode);
    }

    private void enableItemorCollider(int actionMode){
        String localPlayer = Networking.LocalPlayer.displayName;
        switch (actionMode){
            case 0:
            if (!wallMode)targetObject.SetActive(false);
            else targetObject.SetActive(true);
            if(Networking.LocalPlayer.isInstanceOwner && allowInstanceOwner){
                if (!wallMode)targetObject.SetActive(true);
                else targetObject.SetActive(false);
            }
            for (int i =0; i < userName.Length; i++){
                if (localPlayer == userName[i]){
                    if (!wallMode)targetObject.SetActive(true);
                    else targetObject.SetActive(false);
                }
            }
            break;
            case 1:
            if (!wallMode)targetObject.GetComponent<Collider>().enabled = false;
            else targetObject.GetComponent<Collider>().enabled = true;
            
            if(Networking.LocalPlayer.isInstanceOwner && allowInstanceOwner){
                if (!wallMode)targetObject.GetComponent<Collider>().enabled = true;
                else targetObject.GetComponent<Collider>().enabled = false;
            }
            for (int i =0; i < userName.Length; i++){
                if (localPlayer == userName[i]){
                    if (!wallMode)targetObject.GetComponent<Collider>().enabled = true;
                    else targetObject.GetComponent<Collider>().enabled = false;
                }
            }
            break;
            default:
            Debug.Log("Item Lock: Action Mode Index Out Of Bound.");
            Debug.Log("Item Lock: Action Modeの入力にエラーを検出しました。");
            break;
        }
    }

}
