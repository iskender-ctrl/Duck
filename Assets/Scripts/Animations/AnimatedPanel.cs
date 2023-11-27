using DG.Tweening;
using UnityEngine;

namespace Animations
{
    [RequireComponent(typeof(RectTransform))]
    public class AnimatedPanel : MonoBehaviour
    {
        [SerializeField] private AnimationCurve curve = new AnimationCurve(new Keyframe[2]
        {
            new Keyframe(0, 0, 2, 2),
            new Keyframe(1, 1, 0.0f, 0.0f)
        });
        
        public float duration = 0.2f;
        private const float DelayDuration = 0.1f;

        private RectTransform _rectTransform; 
        
        [SerializeField, Range(0, 25)] private int order = 0;

        
        private RectTransform RectTransform
        {
            get
            {
                if (_rectTransform == null)
                {
                    _rectTransform = GetComponent<RectTransform>();
                }

                return _rectTransform;
            }
        }
        
        private void OnEnable()
        {
            RectTransform.DOComplete();
            RectTransform.localScale = Vector3.zero;
            RectTransform.DOScale(Vector3.one, duration).SetEase(curve).SetDelay(order * DelayDuration);
        }

    }
}
