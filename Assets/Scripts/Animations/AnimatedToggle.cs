using UnityEngine;
using UnityEngine.UI;

namespace Animations
{
    /// <summary>
    /// this class handles animations of the ui toggle (can be used as a component on toggles to add different animations to them)
    /// </summary>
    [RequireComponent(typeof(AnimatedButton))]
    public class AnimatedToggle : MonoBehaviour
    {
        /// <summary>
        /// activation status 
        /// </summary>
        public bool isActive = true;

        /// <summary>
        /// toggle image
        /// </summary>
        [SerializeField] private Image image;

        /// <summary>
        /// color for active situation
        /// </summary>
        public Color activeColor;

        /// <summary>
        /// color for deactive situation
        /// </summary>
        public Color deactiveColor;

        /// <summary>
        /// sprite for active situation
        /// </summary>
        public Sprite activeSprite;

        /// <summary>
        /// sprite for deactive situation
        /// </summary>
        public Sprite deactiveSprite;

        public enum ButtonMode
        {
            ChangeColor,
            ChangeSprite,
        }

        public ButtonMode buttonMode;

        public void Toggle(bool active, bool playAnimation = true)
        {
            switch (buttonMode)
            {
                case ButtonMode.ChangeColor:
                    ToggleByChangeColor(active);
                    break;
                case ButtonMode.ChangeSprite:
                    ToggleByChangeSprite(active, playAnimation);
                    break;
            }
        }

        private void ToggleByChangeColor(bool active)
        {
            isActive = active;
            image.color = active ? activeColor : deactiveColor;
        }

        private void ToggleByChangeSprite(bool active, bool playAnimation = true)
        {
            if (playAnimation)
            {
                isActive = active;
                gameObject.SetActive(false);
                image.sprite = active ? activeSprite : deactiveSprite;
                gameObject.SetActive(true);
            }
            else
            {
                isActive = active;
                image.sprite = active ? activeSprite : deactiveSprite;
            }
        }
    }
}