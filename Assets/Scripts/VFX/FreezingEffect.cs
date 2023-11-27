using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VFX
{
    public class FreezingEffect : MonoBehaviour
    {
        [SerializeField] private Image img;
        [SerializeField] private List<Sprite> sprites;
    
        private float _animDuration;

        public void Play(float duration = 5)
        {
            _animDuration = duration;
            StartCoroutine(Animate(sprites, img));
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
}
