using DG.Tweening;
using UnityEngine;

namespace Animations
{
    public class FloatingTransform : MonoBehaviour
    {
        [SerializeField] private float offset;
        [SerializeField] private float moveDuration;
        [SerializeField] private float minAlpha;
        [SerializeField] private float fadeDuration;
        [SerializeField] private SpriteRenderer sprite;
        
        private void Start()
        {
            sprite.DOFade(minAlpha, fadeDuration).SetDelay(1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            transform.DOLocalMoveY(transform.localPosition.y + offset, moveDuration).SetDelay(1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }
    }
}