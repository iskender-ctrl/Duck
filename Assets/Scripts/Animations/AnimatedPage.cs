using System;
using DG.Tweening;
using UnityEngine;

// ReSharper disable MemberCanBePrivate.Global

namespace Animations
{
	/// <summary>
	/// flag enum to specify the animation Behaviour(s)  
	/// </summary>
	[Flags]
	public enum PageBehaviour
	{
		None = 0,
		SlideDown = 1 << 0,
		SlideUp = 1 << 1,
		SlideRight = 1 << 2,
		SlideLeft = 1 << 3,
		Fade = 1 << 4,
	}

	/// <summary>
	/// this class handles animations of the ui page (can be used as a component on pages to add different animations to them)
	/// </summary>
	[RequireComponent(typeof(RectTransform))]
	public class AnimatedPage : MonoBehaviour
	{
		/// <summary>
		/// a SerializeField animation curve to change animation curves OnEnable
		/// </summary>
		[SerializeField] private AnimationCurve enableCurve = new AnimationCurve(new Keyframe[2]
		{
			new Keyframe(0, 0, 2, 2),
			new Keyframe(1, 1, 0.0f, 0.0f)
		});

		/// <summary>
		/// a SerializeField animation curve to change animation curves OnDisable 
		/// </summary>
		[SerializeField] private AnimationCurve disableCurve = new AnimationCurve(new Keyframe[2]
		{
			new Keyframe(0, 0, 0, 0),
			new Keyframe(1, 1, 2.0f, 2.0f)
		});

		/// <summary>
		/// on enable animation behaviour(s)
		/// </summary>
		[SerializeField] private PageBehaviour enableBehaviour = PageBehaviour.Fade | PageBehaviour.SlideUp;

		/// <summary>
		/// on disable animation behaviour(s)
		/// </summary>
		[SerializeField] private PageBehaviour disableBehaviour = PageBehaviour.Fade | PageBehaviour.SlideDown;

		private const float Duration = 0.3f;
		private RectTransform _rectTransform;

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

		private CanvasGroup _canvasGroup;

		private CanvasGroup CanvasGroup
		{
			get
			{
				if (_canvasGroup != null) return _canvasGroup;
				_canvasGroup = GetComponent<CanvasGroup>();
				if (_canvasGroup != null) return _canvasGroup;
				_canvasGroup = gameObject.AddComponent<CanvasGroup>();
				return _canvasGroup;
			}
		}

		private void OnEnable()
		{
			Enable();
		}

		/// <summary>
		/// enable game objects with animation
		/// </summary>
		public void Enable()
		{
			if (enableBehaviour == PageBehaviour.None) return;
			var (disableMinAnchor, disableMaxAnchor) = ComputeAnchors(enableBehaviour);
			RectTransform.DOComplete();
			RectTransform.anchorMin = disableMinAnchor;
			RectTransform.anchorMax = disableMaxAnchor;
			RectTransform.offsetMin = Vector2.zero;
			RectTransform.offsetMax = Vector2.zero;
			RectTransform.DOAnchorMin(Vector2.zero, Duration).SetEase(enableCurve);
			RectTransform.DOAnchorMax(Vector2.one, Duration).SetEase(enableCurve);
			if ((enableBehaviour & PageBehaviour.Fade) == 0)
			{
				CanvasGroup.alpha = 1;
				return;
			}

			CanvasGroup.DOComplete();
			CanvasGroup.alpha = 0;
			CanvasGroup.DOFade(1, Duration).SetEase(enableCurve);
		}

		/// <summary>
		/// disable game object with animation
		/// </summary>
		public void Disable()
		{
			if (disableBehaviour == PageBehaviour.None)
			{
				gameObject.SetActive(false);
				return;
			}

			var (disableMinAnchor, disableMaxAnchor) = ComputeAnchorsReverse(disableBehaviour);
			RectTransform.DOComplete();
			var onCompleteCalled = false;
			if ((enableBehaviour & ~PageBehaviour.Fade) != 0)
			{
				RectTransform.anchorMin = Vector2.zero;
				RectTransform.anchorMax = Vector2.one;
				RectTransform.offsetMin = Vector2.zero;
				RectTransform.offsetMax = Vector2.zero;
				RectTransform.DOAnchorMin(disableMinAnchor, Duration).SetEase(disableCurve);
				RectTransform.DOAnchorMax(disableMaxAnchor, Duration).SetEase(disableCurve)
					.OnComplete(() => { gameObject.SetActive(false); });
				onCompleteCalled = true;
			}

			if ((disableBehaviour & PageBehaviour.Fade) == 0)
			{
				CanvasGroup.alpha = 1;
				return;
			}

			CanvasGroup.DOComplete();
			CanvasGroup.alpha = 1;
			if (onCompleteCalled) CanvasGroup.DOFade(0, Duration).SetEase(disableCurve);
			else
				CanvasGroup.DOFade(0, Duration).SetEase(disableCurve)
					.OnComplete(() => { gameObject.SetActive(false); });
		}


		private static (Vector2, Vector2) ComputeAnchors(PageBehaviour behaviour)
		{
			var disableMinX = (behaviour & PageBehaviour.SlideLeft) != 0 ? 1
			                  : (behaviour & PageBehaviour.SlideRight) != 0 ? -1 : 0;
			var disableMaxX = (behaviour & PageBehaviour.SlideLeft) != 0 ? 2
			                  : (behaviour & PageBehaviour.SlideRight) != 0 ? 0 : 1;
			var disableMinY = (behaviour & PageBehaviour.SlideDown) != 0 ? 1
			                  : (behaviour & PageBehaviour.SlideUp) != 0 ? -1 : 0;
			var disableMaxY = (behaviour & PageBehaviour.SlideDown) != 0 ? 2
			                  : (behaviour & PageBehaviour.SlideUp) != 0 ? 0 : 1;
			var disableMinAnchor = new Vector2(disableMinX, disableMinY);
			var disableMaxAnchor = new Vector2(disableMaxX, disableMaxY);

			return (disableMinAnchor, disableMaxAnchor);
		}

		private static (Vector2, Vector2) ComputeAnchorsReverse(PageBehaviour behaviour)
		{
			var disableMinX = (behaviour & PageBehaviour.SlideRight) != 0 ? 1
			                  : (behaviour & PageBehaviour.SlideLeft) != 0 ? -1 : 0;
			var disableMaxX = (behaviour & PageBehaviour.SlideRight) != 0 ? 2
			                  : (behaviour & PageBehaviour.SlideLeft) != 0 ? 0 : 1;
			var disableMinY = (behaviour & PageBehaviour.SlideUp) != 0 ? 1
			                  : (behaviour & PageBehaviour.SlideDown) != 0 ? -1 : 0;
			var disableMaxY = (behaviour & PageBehaviour.SlideUp) != 0 ? 2
			                  : (behaviour & PageBehaviour.SlideDown) != 0 ? 0 : 1;
			var disableMinAnchor = new Vector2(disableMinX, disableMinY);
			var disableMaxAnchor = new Vector2(disableMaxX, disableMaxY);

			return (disableMinAnchor, disableMaxAnchor);
		}
	}
}
