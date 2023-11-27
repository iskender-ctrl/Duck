using System;
using System.Collections;
using System.Collections.Generic;
using PVE;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class DuckAnimController : MonoBehaviour
    {
        public bool isFrozen = false;
        
        [SerializeField] private float duration = 1f;
        [SerializeField] private Image image;
        [SerializeField] private List<AnimInput> animations;


        private void OnEnable()
        {
            PlayAnim(DuckAnim.Idle);
        }

        /// <summary>
        /// play youe intended animation by type
        /// </summary>
        /// <param name="clip"></param>
        public void PlayAnim(DuckAnim clip)
        {
            var loop = clip == DuckAnim.Idle;
            StopAllCoroutines();
            var anim = animations.Find(x => x.animationCLip == clip);
            if (anim != null)
            {
                StartCoroutine(loop ? PlayLoop(anim.images) : PlayOnce(anim.images));
            }
            else
            {
                Debug.LogError($"animation clip on the {gameObject.name} is null!");
            }
        } 
        

        /// <summary>
        /// animation coroutine. sets image from image list by a certain delay between each one
        /// </summary>
        /// <returns></returns>
        private IEnumerator PlayLoop(List<Sprite> images)
        {
            var delay = duration / images.Count;
            while (true)
            {
                foreach (var img in images)
                {
                    image.sprite = img;
                    yield return new WaitForSeconds(delay);

                    while (isFrozen)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                }
            }
        }
        
        /// <summary>
        /// animation coroutine. sets image from image list by a certain delay between each one
        /// </summary>
        /// <param name="images"></param>
        /// <returns></returns>
        private IEnumerator PlayOnce(List<Sprite> images)
        {
            var delay = duration / images.Count;
            foreach (var img in images)
            {
                image.sprite = img;
                yield return new WaitForSeconds(delay);
                
                while (isFrozen)
                {
                    yield return new WaitForEndOfFrame();
                }
            }

            PlayAnim(DuckAnim.Idle);
        }
        
        /// <summary>
        /// on disable stops animation coroutine
        /// </summary>
        private void OnDisable()
        {
            // StopCoroutine(nameof(PlayLoop));
            StopAllCoroutines();
        }
    }
    
    [Serializable]
    public class AnimInput
    {
        public DuckAnim animationCLip;
        public List<Sprite> images;
    }
        
    [Serializable]
    public enum DuckAnim
    {
        Idle = 0,
        Angry = 1,
        Scared,
    }
}
