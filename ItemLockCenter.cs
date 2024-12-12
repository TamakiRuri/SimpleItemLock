
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ItemLockCenter : UdonSharpBehaviour
{
    [SerializeField] private String[] usernames;
    [SerializeField] private GameObject[] targetObjects;


    [Header("Mode[0]アイテムが消える、[1]アイテムが触れなくなる")]
    [Header("予め[0]オブジェクト[1]コライダーを無効にするとよりセキュアになります")]
    [Header("全ての操作がJoin時に終わるため")]
    [Header("スイッチでオブジェクト(コライダー)を有効にするとロックが解除されます。")]


    [Header("Mode 0 will make the item disappear, 1 will make the item not touchable")]
    [Header("Deactivating[0]objects[1]colliders before uploading is recommanded for better security")]
    [Header("However, the item will be unlocked if a switch enables the object(collider) directly")]

    [Header(" ")]


    [SerializeField] private int actionMode = 0;
    [SerializeField] private bool allowInstanceOwner = false;

    [Header("Wall Modeでは、動作が逆になります（壁などを一部の人だけがぬけるようにするなど）")]
    [Header("In Wall Mode the function of the script become reversed (for creating walls that can only be go through by whitelisted users).")]
    [Header(" ")]
    [SerializeField] private bool wallMode = false;
    private bool shouldOn = false;
    void Start()
    {
        shouldOn = UserCheck();
        // (shouldOn && !wallMode) || (!shouldOn && wallMode)
        // in non wall mode, output == shouldOn
        // in wall mode, output == !shouldOn
        foreach (GameObject _gameObject in targetObjects)
        {
            ScriptAction(_gameObject, actionMode, (shouldOn && !wallMode) || (!shouldOn && wallMode));
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
    private void ScriptAction(GameObject _gameObject, int mode, bool targetState)
    {
        switch (mode)
        {
            case 0:
                _gameObject.SetActive(targetState);
                break;
            case 1:
                _gameObject.GetComponent<Collider>().enabled = targetState;
                break;
            case 2:
                ColliderRecursive(_gameObject, targetState);
                break;
            case 3:
                ColliderRecursive(_gameObject, targetState);
                MeshRendererRecursive(_gameObject, targetState);
                SkinnedMeshRendererRecursive(_gameObject, targetState);
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
        Debug.Log("Username Imported");
    }
    public String[] ExportUsernames()
    {
        return usernames;
    }
#endif
}