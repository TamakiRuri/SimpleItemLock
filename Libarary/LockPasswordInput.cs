
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class LockPasswordInput : UdonSharpBehaviour
{
    [SerializeField] private int digit;
    [SerializeField] private LibPasswordPanel targetPanel;
    void Start()
    {
        if (targetPanel == null) targetPanel = gameObject.GetComponentInParent<LibPasswordPanel>();
    }
    public void InputDigit(){
        targetPanel.AddDigits(digit);
    }
}
