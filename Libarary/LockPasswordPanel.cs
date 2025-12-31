
using System;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class LockPasswordPanel : LibPasswordPanel
{
    public void SubmitPassword(){
        int[] l_results = new int[lockObjects.Length];
        for (int i = 0; i < lockObjects.Length; i++){
            ItemLockCenter targetCenter = lockObjects[i].GetComponent<ItemLockCenter>();
            ItemLockCenterAdvanced targetAdvanced = lockObjects[i].GetComponent<ItemLockCenterAdvanced>();
            if (targetCenter!= null){
                if (i == 0 && mode == 0) {
                    int l_result;
                    l_result = targetCenter.PasswordCheck(ExportInputData());
                    UpdateDisplay(l_result);
                }
                else l_results[i] = targetCenter.PasswordCheck(ExportInputData());
            }
            else if (targetAdvanced!= null){
                if (i == 0 && mode == 0) {
                    int l_result;
                    l_result = targetAdvanced.PasswordCheck(ExportInputData());
                    UpdateDisplay(l_result);
                }
                else l_results[i] = targetAdvanced.PasswordCheck(ExportInputData());
            }
            else{
                Debug.LogError("Item Lock Password Panel: Cannot Find Item Locks. Index:" + i + "\n Item Lockが見つかりません。正しいオブジェクトが入っているのか確認してください");
                return;
            }
        }
        for (int i = 0; i < l_results.Length; i++){
            switch (mode){
                case 0: break;
                case 1:
                    if (l_results[i] == 0) UpdateDisplay(0);
                    break;
                case 2:
                    if (l_results[i] == 1) UpdateDisplay(1);
                    break;
            }
        }
        savedPassword = new String[0];
    }
    
}
