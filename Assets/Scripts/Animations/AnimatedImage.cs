using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Animations
{
    /// <summary>
    /// animates by a list of images 
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class AnimatedImage : MonoBehaviour
    {
        [SerializeField] private List<Sprite> images;
        [SerializeField] private float duration = 1f;
        [SerializeField] private bool playOnAwake = true;
        [SerializeField] private bool loop = true;

        private Image _image;

        private Image Image
        {
            get
            {
                if (_image == null)
                {
                    _image = GetComponent<Image>();
                }

                return _image;
            }
        }

        /// <summary>
        /// on enable starts animation coroutine
        /// </summary>
        private void OnEnable()
        {
            if (playOnAwake)
            {
                StartCoroutine(nameof(Play));
            }
        }

        /// <summary>
        /// on disable stops animation coroutine
        /// </summary>
        private void OnDisable()
        {
            StopCoroutine(nameof(Play));
        }

        /// <summary>
        /// animation coroutine. sets image from image list by a certain delay between each one
        /// </summary>
        /// <returns></returns>
        public IEnumerator Play()
        {
            var delay = duration / images.Count;
            while (loop)
            {
                foreach (var image in images)
                {
                    Image.sprite = image;
                    yield return new WaitForSeconds(delay);
                }
            }
        }
    }
}