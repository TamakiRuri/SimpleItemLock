#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;


using VRC.Udon;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ImportSettings : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;
    [SerializeField]private VisualTreeAsset itemListTemplate;

    [SerializeField]private GameObject LockCenterPrefab;

    [MenuItem("Studio Saphir/Item Lock Settings")]
    public static void ShowExample()
    {
        ImportSettings wnd = GetWindow<ImportSettings>();
        wnd.titleContent = new GUIContent("Item Lock Settings");
    }
    List<String> userLists=new List<String>();
    List<ItemList> itemLists=new List<ItemList>();

    List<String> userListFinal=new List<String>();
    List<GameObject> targetItemFinal = new List<GameObject>();
    List<int> actionModeFinal = new List<int>();
    List<bool> allowOwnerFinal = new List<bool>();

    GameObject controlCenter;

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label("Import");
        root.Add(label);

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);

        //setup target object (NOT user prefabs)
        controlCenter = GameObject.Find("ItemLockCenter(Managed)");
        if (controlCenter != null){
            ReloadItemData();
        }

        //add Username
        var userList = root.Q<ListView>("username");
        userList.makeItem = () => new TextField();
        userList.bindItem = (e,i)=>{
            (e as TextField).label = "Username";
            (e as TextField).multiline = false;
            (e as TextField).isDelayed = true;
            (e as TextField).name = "user";
            };
        userList.itemsSource = userLists;

        //add items
        
        var targetList = root.Q<ListView>("item-list");
        targetList.makeItem = itemListTemplate.CloneTree;
        targetList.bindItem = (e,i)=>{};
        targetList.itemsSource = itemLists;

        // Load Userdata if possible
        if (controlCenter != null){
            ReloadItemData();
        }

        SetupButtonHandler();
    }
    [Serializable]private struct ItemList{
        public ItemList(GameObject target=null, int action=0, bool allow=false){
            targetObject = target;
            actionMode = action;
            allowInstanceOwner = allow;
        }
        public GameObject targetObject;
        public int actionMode;
        public bool allowInstanceOwner;
    }
        private void ReloadItemData(){
        VisualElement root = rootVisualElement;
        root.Q<Label>("result").text = "Wait";
        String[] l_usernames = controlCenter.GetComponent<ItemLockDatabase>().exportUserData();
        GameObject[] l_objects = controlCenter.GetComponent<ItemLockDatabase>().exportObjectData();
        int[] l_modes = controlCenter.GetComponent<ItemLockDatabase>().exportModeData();
        bool[] l_allowOwner = controlCenter.GetComponent<ItemLockDatabase>().exportAllowOwnerData();
        ItemList[] l_list = new ItemList[l_objects.Length];
        for (int i = 0; i < l_objects.Length; i++){
            l_list[i] = new ItemList(l_objects[i], l_modes[i], l_allowOwner[i]);
        }
        itemLists = l_list.ToList();
        userLists = l_usernames.ToList();
        foreach (ItemList ll_list in itemLists){
            Debug.Log("Game Object " + ll_list.targetObject + " Mode " + ll_list.actionMode + " Allow Owner " +  ll_list.allowInstanceOwner + " Loaded");
        }
        //add users
        var userList = root.Q<ListView>("username");
        userList.bindItem = (e,i)=>{
            (e as TextField).label = "Username";
            (e as TextField).multiline = false;
            (e as TextField).isDelayed = true;
            (e as TextField).name = "user";
            (e as TextField).value = userLists[i];
            };
        //add items
        var targetList = root.Q<ListView>("item-list");
        targetList.itemsSource = itemLists;
        targetList.bindItem = (e,i)=>{
            (e.ElementAt(0).ElementAt(0) as ObjectField).value = itemLists[i].targetObject;
            (e.ElementAt(0).ElementAt(1) as IntegerField).value = itemLists[i].actionMode;
            (e.ElementAt(0).ElementAt(2) as BaseBoolField).value = itemLists[i].allowInstanceOwner;
        };
        root.Q<Label>("result").text = "Data Loaded from Local Database";
    }
    private void SetupButtonHandler(){
        VisualElement root = rootVisualElement;

        var buttons = root.Query<Button>();
        buttons.ForEach(RegisterHandler);
    }
    private void RegisterHandler(Button button){
        if (button.name == "generate-data")
        button.RegisterCallback<ClickEvent>(GenerateLockData);

        else if (button.name == "delete-data")
        button.RegisterCallback<ClickEvent>(DeleteLockData);
    }

    private void UsernameHandler(TextField text){
        userListFinal.Add(text.value);
    }
    private void TargetObjectHandler(VisualElement obj){
        targetItemFinal.Add((GameObject)obj.Q<ObjectField>().value);
        actionModeFinal.Add(obj.Q<IntegerField>().value);
        allowOwnerFinal.Add(obj.Q<BaseBoolField>().value);
    }
    //data related functions not implemented
    private void GenerateLockData(ClickEvent _event){
        VisualElement root = rootVisualElement;
        root.Q<Label>("result").text = "Wait";

        userListFinal.Clear();
        targetItemFinal.Clear();
        actionModeFinal.Clear();
        allowOwnerFinal.Clear();

        try {
        root.Query<TextField>("user").ForEach(UsernameHandler);
        root.Query<VisualElement>("iltemplate").ForEach(TargetObjectHandler);
        }
        catch (Exception e){
            throw e;
        }
        if (controlCenter != null) DestroyImmediate(controlCenter);
        controlCenter = Instantiate(LockCenterPrefab);
        controlCenter.name = "ItemLockCenter(Managed)";
        generateDatatoCenter();
        root.Q<Label>("result").text = "Finished";
        Debug.Log("Generate Finished. \"Should Run Behavior\" Errors are safe to ignore.");
    }
    private void generateDatatoCenter(){
        var controlCenterUdon = (UdonBehaviour)controlCenter.GetComponent(typeof(UdonBehaviour));
        String[] users = userListFinal.ToArray();
        controlCenterUdon.SendMessage("importUsernames", users);
        controlCenter.GetComponent<ItemLockDatabase>().importUsernames(users);
        GameObject[] gameobjs = targetItemFinal.ToArray();
        controlCenterUdon.SendMessage("importTargets", gameobjs);
        int[] modes = actionModeFinal.ToArray();
        controlCenterUdon.SendMessage("importModes", modes);
        bool[] allowowners = allowOwnerFinal.ToArray();
        controlCenterUdon.SendMessage("importAllowOwner", allowowners);
        controlCenter.GetComponent<ItemLockDatabase>().importObjectData(gameobjs, modes, allowowners);
    }
    //data related functions not implemented
    private void DeleteLockData(ClickEvent _event){
        VisualElement root = rootVisualElement;
        root.Q<Label>("result").text = "Wait";
        if (controlCenter != null){
            DestroyImmediate(controlCenter);
            root.Q<Label>("result").text = "Finished";
        }
        else {root.Q<Label>("result").text = "No Control Center Found";}
        
    }
    
}
#endif