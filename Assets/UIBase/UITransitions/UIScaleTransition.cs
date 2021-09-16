using DG.Tweening;
using BasicUI.UI.Core;
using System;
using UnityEngine;
namespace UI
{
    public class UIScaleTransition : UITransition
    {
        [SerializeField]
        private Vector3 _startScale;
        [SerializeField] private float _showDelay;
        private Vector3 _finishScale;
        private Tweener _activeTweener;

        protected override void OnShow(Action FinishCallback)
        {
            Reset();
            _activeTweener = _rectTransform.DOScale(_finishScale, _showTransitionDuration).SetDelay(_showDelay)
                .OnComplete(() => FinishCallback?.Invoke());
        }

        protected override void OnHide(Action FinishCallback)
        {
            _activeTweener = _rectTransform.DOScale(_startScale, _hideTransitionDuration)
                .OnComplete(() => FinishCallback?.Invoke());
        }

        public override void Reset()
        {
            _rectTransform.localScale = _startScale;
        }

        public override void Initialize(float showTransitionDuration, float hideTransitionDuration)
        {
            base.Initialize(showTransitionDuration, hideTransitionDuration);
            _finishScale = _rectTransform.localScale;
        }

        protected override void OnAbort()
        {
            if(_activeTweener != null && _activeTweener.IsPlaying())
                _activeTweener.Kill();
        }
    }
}