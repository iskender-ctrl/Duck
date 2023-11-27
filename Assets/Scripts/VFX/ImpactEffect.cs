using System;
using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;
using UnityEngine.UI;

namespace VFX
{
    public class ImpactEffect : MonoBehaviour
    {
        [SerializeField] private Image img;
        private float _animDuration;
        

        public void Play(ImpactType type, float duration = .3f)
        {
            if (type is ImpactType.None)
                return;
            
            _animDuration = duration;
            var anim = GameData.Instance.GameAssets.explosionAnim.Find(x => x.type == type).anim;
            StartCoroutine(Animate(anim, img));
        }
        
        private IEnumerator Animate(List<Sprite> sprites, Image imgReference)
        {
            var delay = _animDuration / sprites.Count;
            
            //destroy animation
            foreach (var img in sprites)
            {
                imgReference.sprite = img;
                yield return new WaitForSeconds(delay);
            }
            
            Destroy(gameObject);
        }
    }

    [Serializable]
    public enum ImpactType
    {
        None = 0,
        Normal = 1,
        
    }
}
