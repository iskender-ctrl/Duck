using UnityEngine;

namespace UI.BattleUI
{
    public class ActionBarController : MonoBehaviour
    {

        [SerializeField] Animator[] AbilityItems;
        [SerializeField] Animator[] ObjectItems;
   

        public void SelectedAbilityItem(int index)
        {
            for (int i = 0; i < AbilityItems.Length; i++)
            {
                AbilityItems[i].SetBool("Open", i==index);
            }

        }

        public void SelectedObject(int index)
        {
            for (int i = 0; i < ObjectItems.Length; i++)
            {
                ObjectItems[i].SetBool("Open", i == index);
            }
        }
    }
}
