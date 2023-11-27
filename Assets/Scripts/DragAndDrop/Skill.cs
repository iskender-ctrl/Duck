using UnityEngine;

public class Skill : Draggable
{
    public enum SkillType
    {
        OneCharacter,
        TwoCharacter,
        TowerHealSkill,
        DamageSkill
        // Ek skill tipleri eklenebilir.
    }

    public SkillType skillType;

    // Diğer özellikler...

    // Örnek bir metot:
    public void UseSkill()
    {
        // Yetenek kullanımı burada gerçekleştirilir.
        Debug.Log("Skill used! Type: " + skillType.ToString());
    }
}
