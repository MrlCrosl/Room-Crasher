using System;
using BasicUI.UI.Core;
using DG.Tweening;
using UnityEngine;

namespace BasicUI.UI
{
    public enum MoveOutDirection
    {
        left,
        right,
        top,
        bottom
    }

    public class UIMoveOutTransition : UITransition
    {
        public MoveOutDirection _moveOutDirection;

        [SerializeField] private Canvas _parentCanvas;
        [SerializeField] private bool _returnToStartPositionAfterHide = true;
        [SerializeField] private float _showDelay;
        private Tweener _activeTween;
        private Vector3 _startPos;
        private Vector3 _hiddenPosition;
        private RectTransform _canvasTransform;

        public float ShowDelay
        {
            get => _showDelay;
            set => _showDelay = value;
        }

        public override void Initialize(float showTransitionDuration, float hideTransitionDuration)
        {
            base.Initialize(showTransitionDuration, hideTransitionDuration);
            _startPos = _rectTransform.anchoredPosition;
            _canvasTransform = _parentCanvas.GetComponent<RectTransform>();
            _hiddenPosition = GetHidePos();
        }

        private Vector2 SwitchToRectTransform(RectTransform to)
        {
            RectTransform from = _canvasTransform;
            Vector2 localPoint;
            Vector2 fromPivotDerivedOffset = new Vector2(from.rect.width * from.pivot.x + from.rect.xMin,
                from.rect.height * from.pivot.y + from.rect.yMin);
            Vector2 screenP = RectTransformUtility.WorldToScreenPoint(null, from.position);
            screenP += fromPivotDerivedOffset;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(to, screenP, null, out localPoint);
            Vector2 pivotDerivedOffset = new Vector2(to.rect.width * to.pivot.x + to.rect.xMin,
                to.rect.height * to.pivot.y + to.rect.yMin);
            return to.anchoredPosition + localPoint - pivotDerivedOffset;
        }

        public override void Reset()
        {
            if (_returnToStartPositionAfterHide)
            {
                _startPos = _rectTransform.anchoredPosition;
            }

            _rectTransform.anchoredPosition = _hiddenPosition;
        }

        protected override void OnShow(Action FinishCallback)
        {
            Reset();
            Tween tween = null;
            switch (_moveOutDirection)
            {
                case MoveOutDirection.top:
                    tween = _rectTransform.DOAnchorPosY(_startPos.y, _showTransitionDuration).SetDelay(_showDelay)
                        .SetEase(_curve);
                    break;
                case MoveOutDirection.bottom:
                    tween = _rectTransform.DOAnchorPosY(_startPos.y, _showTransitionDuration).SetDelay(_showDelay)
                        .SetEase(_curve);
                    ;
                    break;
                case MoveOutDirection.left:
                    tween = _rectTransform.DOAnchorPosX(_startPos.x, _showTransitionDuration).SetDelay(_showDelay)
                        .SetEase(_curve);
                    ;
                    break;
                case MoveOutDirection.right:
                    tween = _rectTransform.DOAnchorPosX(_startPos.x, _showTransitionDuration).SetDelay(_showDelay)
                        .SetEase(_curve);
                    ;
                    break;
            }

            //tween.SetUpdate(true);
            tween.OnComplete(() => FinishCallback?.Invoke());
        }

        protected override void OnHide(Action FinishCallback)
        {
            Tweener tween = null;
            switch (_moveOutDirection)
            {
                case MoveOutDirection.top:
                    tween = _rectTransform.DOAnchorPosY(GetHidePos().y, _hideTransitionDuration).SetDelay(_showDelay);
                    break;
                case MoveOutDirection.bottom:
                    tween = _rectTransform.DOAnchorPosY(GetHidePos().y, _hideTransitionDuration).SetDelay(_showDelay);
                    break;
                case MoveOutDirection.left:
                    tween = _rectTransform.DOAnchorPosX(GetHidePos().x, _hideTransitionDuration).SetDelay(_showDelay);
                    break;
                case MoveOutDirection.right:
                    tween = _rectTransform.DOAnchorPosX(GetHidePos().x, _hideTransitionDuration).SetDelay(_showDelay);
                    break;
            }

           // tween.SetUpdate(true);
            tween.OnComplete(() =>
            {
                FinishCallback?.Invoke();
                OnHideTransitionComplete();
            });
            _activeTween = tween;
        }

        private Vector2 GetHidePos()
        {
            int divider = 2;
            if (_rectTransform.anchorMax == Vector2.one && _rectTransform.anchorMin == Vector2.zero)
                divider = 1;
            Vector2 hideVector = Vector2.zero;
            switch (_moveOutDirection)
            {
                case MoveOutDirection.top:
                    hideVector = new Vector2(_rectTransform.anchoredPosition.x,
                        SwitchToRectTransform(_rectTransform).y + _canvasTransform.sizeDelta.y / divider +
                        _rectTransform.sizeDelta.y);
                    break;
                case MoveOutDirection.bottom:
                    hideVector = new Vector2(_rectTransform.anchoredPosition.x,
                        SwitchToRectTransform(_rectTransform).y - _canvasTransform.sizeDelta.y / divider -
                        _rectTransform.sizeDelta.y);
                    break;
                case MoveOutDirection.left:
                    hideVector =
                        new Vector2(
                            SwitchToRectTransform(_rectTransform).x - _canvasTransform.sizeDelta.x / divider -
                            _rectTransform.sizeDelta.x, _rectTransform.anchoredPosition.y);
                    break;
                case MoveOutDirection.right:
                    hideVector =
                        new Vector2(
                            SwitchToRectTransform(_rectTransform).x + _canvasTransform.sizeDelta.x / divider +
                            _rectTransform.sizeDelta.x, _rectTransform.anchoredPosition.y);
                    break;
            }

            return hideVector;
        }


        protected override void OnAbort()
        {
            if (_activeTween != null)
            {
                _activeTween.Kill(true);
            }
        }

        private void OnHideTransitionComplete()
        {
            _rectTransform.anchoredPosition = _hiddenPosition;
            if (_returnToStartPositionAfterHide)
            {
                _rectTransform.anchoredPosition = _startPos;
            }
        }
    }
}