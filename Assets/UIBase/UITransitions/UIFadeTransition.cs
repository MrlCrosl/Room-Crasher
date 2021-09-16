using DG.Tweening;
using System;
using BasicUI.UI.Core;
using UnityEngine;
namespace BasicUI.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIFadeTransition : UITransition
    {
        private CanvasGroup _canvasGroup;
        private Tweener _activeTweener;
        private bool _finished;

        protected override void OnShow(Action FinishCallback)
        {
            Reset();
            _activeTweener = _canvasGroup.DOFade(1f, _showTransitionDuration).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    _canvasGroup.interactable = true;
                    FinishCallback?.Invoke();
                });
        }

        protected override void OnHide(Action FinishCallback)
        {
            _canvasGroup.interactable = false;
            _activeTweener = _canvasGroup.DOFade(0, _hideTransitionDuration).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    FinishCallback?.Invoke();
                });
        }
        
        public override void Reset()
        {
            _canvasGroup.interactable = false;
            _canvasGroup.alpha = 0;
        }

        public override void Initialize(float showTransitionDuration, float hideTransitionDuration)
        {
            base.Initialize(showTransitionDuration, hideTransitionDuration);
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        protected override void OnAbort()
        {
            if (_activeTweener != null && _activeTweener.IsPlaying())
                _activeTweener.Kill();
        }
		
		public override void HideWithoutAnimation()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
        }
    }
}