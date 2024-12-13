
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ItemLockCenter : UdonSharpBehaviour
{
    [SerializeField] private String[] usernames;
    [SerializeField] private GameObject[] targetObjects;


    [Header("Modeに関する詳しい説明は、Githubおよび商品ページにあります。")]

    [Header("Specific Inforamtion is on the github and booth page")]

    [Header("Github: https://github.com/TamakiRuri/SimpleItemLock")]

    [Header("Booth: https://saphir.booth.pm/items/6375850")]

    [Header(" ")]

    [SerializeField] private int actionMode = 0;
    [SerializeField] private bool allowInstanceOwner = false;
    [SerializeField] private bool wallMode = false;
    private bool shouldOn = false;
    void Start()
    {
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
    private void ScriptAction(GameObject targetObject, int mode, bool targetState)
    {
        switch (mode)
        {
            case 0:
                targetObject.SetActive(targetState);
                break;
            case 1:
                Collider targetCollider = targetObject.GetComponent<Collider>();
                if (targetCollider == null)
                {
                    Debug.LogError("Item Lock: This object is in Action Mode 1 but the Collider can't be detected");
                    Debug.LogError("Item Lock: このオブジェクトの動作モードがAction Mode 1ですがコライダーを取得できません");
                }
                else
                    targetCollider.enabled = targetState;
                break;
            case 2:
                ColliderRecursive(targetObject, targetState);
                break;
            case 3:
                ColliderRecursive(targetObject, targetState);
                MeshRendererRecursive(targetObject, targetState);
                SkinnedMeshRendererRecursive(targetObject, targetState);
                break;
            default:
                Debug.LogError("Item Lock: Action Mode Index Out Of Bound.");
                Debug.LogError("Item Lock: Action Modeの入力にエラーを検出しました。");
                break;
        }

    }
    private void SkinnedMeshRendererRecursive(GameObject targetObject, bool targetState)
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
    private void MeshRendererRecursive(GameObject targetObject, bool targetState)
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
    private void ColliderRecursive(GameObject targetObject, bool targetState)
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
            Debug.LogError("Item Lock: No colliders found");
        }
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