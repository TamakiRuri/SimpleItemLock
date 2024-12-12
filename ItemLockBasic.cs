
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ItemLockBasic : UdonSharpBehaviour
{
    [Header("何も入れない場合、現在のオブジェクトが設定されます。")]
    [Header("If no object is attached, current object will be automatically set up")]
    [Header(" ")]
    [SerializeField] private GameObject targetObject;
    [SerializeField] private String[] usernames;

    [Header("Mode[0]アイテムが消える、[1]アイテムが触れなくなる")]
    [Header("[1]予めコライダーを無効にするとよりセキュアになります")]
    [Header("[0]このスクリプトがオブジェクトにアタッチしたまま無効にしないでください")]
    [Header("スイッチでオブジェクト(コライダー)を有効にするとロックが解除されます。")]


    [Header("Mode 0 will make the item disappear, 1 will make the item not touchable")]
    [Header("[1]Deactivating colliders before uploading is recommanded for better security")]
    [Header("[0]Don't deactivate the game object if this script is attacted to it.")]
    [Header("The item will be unlocked if a switch enables the object(collider) directly")]

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
        shouldOn = UserCheck();
        if (actionMode == 1)
        {
            targetCollider = targetObject.GetComponent<Collider>();
            if (targetCollider == null)
            {
                Debug.LogError("Item Lock: This object is in Action Mode 1 but the Collider can't be detected");
                Debug.LogError("Item Lock: このオブジェクトの動作モードがAction Mode 1ですがコライダーを取得できません");
                return;
            }
        }
        // (shouldOn && !wallMode) || (!shouldOn && wallMode)
        // in non wall mode, output == shouldOn
        // in wall mode, output == !shouldOn
        ScriptAction(actionMode, (shouldOn && !wallMode) || (!shouldOn && wallMode));
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

    private void ScriptAction(int mode, bool targetState)
    {
        switch (mode)
        {
            case 0:
                targetObject.SetActive(targetState);
                break;
            case 1:
                targetObject.GetComponent<Collider>().enabled = targetState;
                break;
            case 2:
                ColliderRecursive(targetState);
                break;
            case 3:
                ColliderRecursive(targetState);
                MeshRendererRecursive(targetState);
                SkinnedMeshRendererRecursive(targetState);
                break;
            // case 4:
            //     ColliderModeRecursive(targetState);
            //     CanvasRecursive(targetState);
            //     RaycasterRecursive(targetState);
            //     break;
            default:
                Debug.LogError("Item Lock: Action Mode Index Out Of Bound.");
                Debug.LogError("Item Lock: Action Modeの入力にエラーを検出しました。");
                break;
        }
    }

    // Case 4 for Canvas Mode
    // Not Actually working

    // private void RaycasterRecursive(bool targetState)
    // {
    //     GraphicRaycaster[] t_raycasters = targetObject.GetComponentsInChildren<GraphicRaycaster>();
    //     if (t_raycasters.Length != 0)
    //     {
    //             foreach (GraphicRaycaster l_raycaster in t_raycasters)
    //             {
    //                 l_raycaster.enabled = targetState;
    //             }
    //     }
    //     else
    //     {
    //         Debug.LogError("Item Lock: No Canvases Found");
    //     }
    // }
    // private void CanvasRecursive(bool targetState)
    // {
    //     Canvas[] t_canvases = targetObject.GetComponentsInChildren<Canvas>();
    //     if (t_canvases.Length != 0)
    //     {
    //             foreach (Canvas l_canvas in t_canvases)
    //             {
    //                 l_canvas.enabled = targetState;
    //             }
    //     }
    //     else
    //     {
    //         Debug.LogError("Item Lock: No Canvases Found");
    //     }
    // }
    private void SkinnedMeshRendererRecursive(bool targetState)
    {
        SkinnedMeshRenderer[] t_meshRenderers = targetObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        if (t_meshRenderers.Length != 0)
        {
            foreach (SkinnedMeshRenderer l_meshRenderer in t_meshRenderers)
            {
                l_meshRenderer.enabled = targetState;
            }
        }
        else
        {
            Debug.LogError("Item Lock: No Skinned Mesh Renderer Found");
        }
    }
    private void MeshRendererRecursive(bool targetState)
    {
        MeshRenderer[] t_meshRenderers = targetObject.GetComponentsInChildren<MeshRenderer>();
        if (t_meshRenderers.Length != 0)
        {
            foreach (MeshRenderer l_meshRenderer in t_meshRenderers)
            {
                l_meshRenderer.enabled = targetState;
            }
        }
        else
        {
            Debug.LogError("Item Lock: No Mesh Renderer Found");
        }
    }
    private void ColliderRecursive(bool targetState)
    {
        Collider[] t_colliders = targetObject.GetComponentsInChildren<Collider>();
        if (t_colliders.Length != 0)
        {
            foreach (Collider l_collider in t_colliders)
            {
                l_collider.enabled = targetState;
            }
        }
        else
        {
            Debug.LogError("Item Lock: No Collider Found");
        }
    }
#if UNITY_EDITOR && !COMPILER_UDONSHARP
    public void ImportUsernames(String[] importedUsernames)
    {
        usernames = importedUsernames;
        Debug.Log("Username Imported");
    }


    public String[] ExportUsernames()
    {
        return usernames;
    }
#endif
}
