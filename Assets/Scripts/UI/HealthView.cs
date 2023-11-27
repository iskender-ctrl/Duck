using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.BattleUI
{
    public class HealthView : MonoBehaviour
    {
    
        [SerializeField] private Slider healthSlider;
        [SerializeField] private HealthValueSprite[] healthValueSprites;
        [SerializeField] private TMP_Text healthText;
        private Image sliderImage;

        private void Awake()
        {
            sliderImage = healthSlider.fillRect.GetComponent<Image>();
        }

        private void OnEnable()
        {
            healthSlider.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnDisable()
        {
            healthSlider.onValueChanged.RemoveListener(OnValueChanged);

        }

        private void OnValueChanged(float value)
        {
            healthText.SetText($"{value}/{healthSlider.maxValue}");
            for (int i = 0; i < healthValueSprites.Length; i++)
            {
                if (healthValueSprites[i].MaxHealth>=healthSlider.normalizedValue)
                {
                    sliderImage.sprite = healthValueSprites[i].Sprite;
                    return;
                }
            }
        }
    }

    [Serializable]
    struct HealthValueSprite
    {
        [Range(0, 1)] public float MaxHealth; 
        public Sprite Sprite;
    }

}

