using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class LibItemLock : UdonSharpBehaviour
{
    protected String[] AddtoArray(String[] l_playerArray, String l_name){
        String[] l_nameList = new string[l_playerArray.Length+1];
        for(int i = 0; i < l_playerArray.Length; i++){
            l_nameList[i] = l_playerArray[i];
        }
        l_nameList[l_playerArray.Length] = l_name;
        return l_nameList;
    }
    protected String[] MergetoArray(String[] l_playerArray, String[] l_names){
        String[] l_nameList = new string[l_playerArray.Length+l_names.Length];
        for(int i = 0; i < l_playerArray.Length; i++){
            l_nameList[i] = l_playerArray[i];
        }
        for (int i = l_playerArray.Length; i < l_playerArray.Length + l_names.Length; i++)
        l_nameList[i] = l_names[i];
        return l_nameList;
    }


    protected String[] RemovefromArray(String[] l_PlayerArray, String l_name){
        String[] l_nameList = new string[l_PlayerArray.Length-1];
        bool isRemoved = false;
        for(int i = 0; i < l_PlayerArray.Length; i++){
            if (!isRemoved && l_name!=l_PlayerArray[i])l_nameList[i] = l_PlayerArray[i];
            else if (l_name==l_PlayerArray[i]){
                isRemoved = true;
            }
            else l_nameList[i-1] = l_PlayerArray[i];
        }
        return l_nameList;
    }
    protected void ScriptAction(GameObject targetObject, int mode, bool targetState)
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
    protected void SkinnedMeshRendererRecursive(GameObject targetObject, bool targetState)
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
    protected void MeshRendererRecursive(GameObject targetObject, bool targetState)
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
    protected void ColliderRecursive(GameObject targetObject, bool targetState)
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
}
