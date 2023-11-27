using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ActionBarController : MonoBehaviour
{
    [Serializable]
    public class AbilityData
    {
        public Sprite icon;
        public int energyCount = 3;
        public EntryType EntryType;
    }

    public enum EntryType
    {
        None,
        Earth,
        Sky
    }
    
    
    [SerializeField] private AbilityData[] AbilityDatas;
    [FormerlySerializedAs("_tempViews")] [SerializeField] private SkillView[] skillViews;
    private int _previousAbilitySelectIndex = -1;
    
    
    [SerializeField] GameObject[] ObjectItems;

    [SerializeField] private RectTransform earthEntriesZoneHighlight;
    [SerializeField] private RectTransform airEntriesZoneHighlight;
    
    Animator anim;

    private void Awake()
    {
        int length = AbilityDatas.Length;
        for (int i = 0; i < length; i++)
        {
            ref var data = ref AbilityDatas[i];
            skillViews[i].Init(i,data.icon,data.energyCount);
            skillViews[i].OnSkillClicked += InstanceOnOnSkillClicked;
        }
    }

    private void InstanceOnOnSkillClicked(int index)
    {
        EntryType entryType = AbilityDatas[index].EntryType;
        
        if(_previousAbilitySelectIndex>-1)
            skillViews[_previousAbilitySelectIndex].Deselect();
        if (_previousAbilitySelectIndex == index)
        {
            _previousAbilitySelectIndex = -1;
            entryType = EntryType.None;
        }

        _previousAbilitySelectIndex = index;
        
        earthEntriesZoneHighlight.gameObject.SetActive(entryType == EntryType.Earth);
        airEntriesZoneHighlight.gameObject.SetActive(entryType == EntryType.Sky);
    }

    public Transform testInstantiateParent;
    private void Update()
    {
        if (_previousAbilitySelectIndex > -1)
        {
            if (Input.GetMouseButtonDown(0))
            {
                var dataEntry = AbilityDatas[_previousAbilitySelectIndex];
                var screenPoint = Input.mousePosition;
                
                if (dataEntry.EntryType == EntryType.Earth)
                {
                    if(RectTransformUtility.RectangleContainsScreenPoint(earthEntriesZoneHighlight, screenPoint,Camera.main))
                        return;
                }
                if (dataEntry.EntryType == EntryType.Sky)
                {
                    if(RectTransformUtility.RectangleContainsScreenPoint(airEntriesZoneHighlight, screenPoint,Camera.main))
                        return;
                }
                
                var testIInst = new GameObject();
                testIInst.layer = LayerMask.NameToLayer("UI");
                testIInst.transform.SetParent(testInstantiateParent);
                testIInst.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var pos = testIInst.transform.localPosition;
                pos.z = 0;
                testIInst.transform.localPosition = pos;
                testIInst.transform.localScale = Vector3.one;
                
                
                
                testIInst.AddComponent<Image>().sprite = dataEntry.icon;
            }
        }
    }

    public void SelectedObject(int index)
    {
        for (int i = 0; i < ObjectItems.Length; i++)
        {
            anim = GetChildAnimator(ObjectItems[i]);
            anim.SetBool("Open", false);
        }

        anim = GetChildAnimator(ObjectItems[index]);
        anim.SetBool("Open", true);
    }
    
    private Animator GetChildAnimator(GameObject parent) =>
        parent.transform.GetChild(0).gameObject.GetComponent<Animator>();
}
