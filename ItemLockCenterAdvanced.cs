
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ItemLockCenterAdvanced : UdonSharpBehaviour
{
    [Header("このScriptのデータは自動生成されます")]
    [Header("アイテムを動かしたりすることは、エラーを引き起こす恐れがあります。")]

    [Header("The data in this script is auto generated")]
    [Header("Adding or moving objects could potentially break the program")]
    [Header(" ")]
    [SerializeField] private String[] usernames;
    [SerializeField] private GameObject[] targetObjects;
    [SerializeField] private int[] actionMode;
    [SerializeField] private bool[] allowInstanceOwner;
    [SerializeField] private bool[] wallModes;

    // allow InstanceOwner is different for each object this only applies to list members
    private bool userInList = false;

    void Start()
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
    private bool UserListCheck()
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

    private void ScriptAction(GameObject targetObject, int mode, bool targetState)
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
        Debug.Log("Username Imported");
    }

    public void ImportLockData(GameObject[] l_gameObject, int[] l_modes, bool[] l_allowOwner, bool[] l_wallModes)
    {
        targetObjects = l_gameObject;
        actionMode = l_modes;
        allowInstanceOwner = l_allowOwner;
        wallModes = l_wallModes;
    }
    public void ImportTargets(GameObject[] importedGameObjects)
    {
        targetObjects = importedGameObjects;
        Debug.Log("Target GameObjects Imported");
    }
    public void ImportModes(int[] importedModes)
    {
        actionMode = importedModes;
        Debug.Log("Action Modes Imported");
    }
    public void ImportAllowOwner(bool[] importedAllowOwner)
    {
        allowInstanceOwner = importedAllowOwner;
        Debug.Log("Allow Instance Owner Settings Imported");
    }
    public void ImportWallModes(bool[] importedWallModes)
    {
        wallModes = importedWallModes;
        Debug.Log("Wall Mode Settings Imported");
    }
    // Export Functions (not in use)
    public String[] ExportUsernames()
    {
        return usernames;
    }

#endif

}