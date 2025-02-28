
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class LibItemLock : UdonSharpBehaviour
{
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
