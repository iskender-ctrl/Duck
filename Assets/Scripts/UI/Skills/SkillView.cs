using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SkillView : MonoBehaviour
{
    private static readonly int Open = Animator.StringToHash("Open");
    public event Action<int> OnSkillClicked;

    [SerializeField] private Button button;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI energyCountText;
    [SerializeField] private Animator animator;

    private int _skillId;

    private void OnEnable() => button.onClick.AddListener(OnClick);

    private void OnDisable() => button.onClick.RemoveListener(OnClick);

    private void OnClick()
    {
        animator.SetBool(Open,!animator.GetBool(Open));
        OnSkillClicked?.Invoke(_skillId);
    }

    public void Deselect()
    {
        animator.SetBool(Open,false);
    }

    public void Init(int id,Sprite icon,int energyCount)
    {
        _skillId = id;
        iconImage.sprite = icon;
        energyCountText.text = energyCount.ToString();
    }
}
