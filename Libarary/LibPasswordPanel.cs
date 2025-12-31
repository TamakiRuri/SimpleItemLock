using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;

public class LibPasswordPanel : UdonSharpBehaviour
{
    [Header("デフォルトのテキスト")]
    [SerializeField] protected String defaultText = "Input Password";
    [Header("認証成功のテキスト")]
    [SerializeField] protected String successText = "Success";
    [Header("認証失敗のテキスト")]
    [SerializeField] protected String failedText = "Failed";
    [Header("すでに解除したときのテキスト")]
    [SerializeField] protected String alreadyUnlockedText = "Already Unlocked";
    [Header("失敗した回数が多すぎてブロックされたときのテキスト")]
    [SerializeField] protected String BlockedText = "Too Many Attempts";
    [Header("モード: 0 一つ目の結果を表示。1 成功のメッセージを優先。2 失敗のメッセージを優先")]
    [Header("Mode: 0 display the first output. 1 prioritize success message. 2 prioritize fail message.")]
    [SerializeField] protected int mode = 0;
    [Header("ItemLockをアタッチしてください")]
    [Header("Password Creatorモードでは、Password Creatorスクリプトが入っているオブジェクトをアタッチしてください。")]
    [Header("Please attch the ItemLocks. ")]
    [Header("Attach the object that password creator is in in Password Creator mode")]
    [SerializeField] protected GameObject[] lockObjects;

    [SerializeField] protected TextMeshProUGUI passwordDisplay;
    protected String[] savedPassword = {};
    void Start()
    {
        if (defaultText == "")defaultText = passwordDisplay.text;
    }
    public String ExportInputData(){
        return String.Concat(savedPassword);
    }
    
    public void UpdateDisplay(int l_value = -2){
        switch (l_value) {
            case -2:
                passwordDisplay.text = ExportInputData();
                break;
            case -1:
                passwordDisplay.text = defaultText;
                break;
            case 0:
                passwordDisplay.text = successText;
                break;
            case 1:
                passwordDisplay.text = failedText;
                break;
            case 2:
                passwordDisplay.text = alreadyUnlockedText;
                break;
            case 3:
                passwordDisplay.text = BlockedText;
                break;
            default:
                passwordDisplay.text = "Error";
                break;
        }
        
    }
    public void AddDigits(int l_input){
        savedPassword = ArrayAdd(savedPassword, l_input.ToString());
        UpdateDisplay();
    }
    public String[] ArrayAdd(String[] l_Array, String l_value){
        if (l_Array.Length < 100){
            String[] o_Array = new String[l_Array.Length + 1];
            for (int i = 0; i < l_Array.Length; i++){
                o_Array[i] = l_Array[i];
            }
            o_Array[l_Array.Length] = l_value;
            return o_Array;
        }
        else
        return l_Array;
    }
    public void ClearPassword(){
        savedPassword = new String[0];
        passwordDisplay.text = defaultText;
    }
}
