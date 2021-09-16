using System;
using DG.Tweening;
using UnityEngine;
namespace BasicUI.UI.Core
{
    [RequireComponent(typeof(RectTransform))]
    [ExecuteInEditMode]
    public abstract class UITransition : MonoBehaviour
    {
        public bool _overrideDuration;
        public float _customShowDuration;
        public float _customHideDuration;
        public bool _executeBeforeChildTransitions;


        [SerializeField, HideInInspector]
        protected RectTransform _rectTransform;
        [SerializeField] 
        protected AnimationCurve _curve;
        protected float _showTransitionDuration;
        protected float _hideTransitionDuration;

        private Ease _showEaseMain;
        private Action _finishCallback;
        private UITransition _parentTransitionToWait;

#if UNITY_EDITOR
        private void OnEnable()
        {
            if (!Application.isPlaying)
                _rectTransform = GetComponent<RectTransform>();
        }
#endif

        public virtual void Initialize(float showTransitionDuration, float hideTransitionDuration)
        {
            if (!_overrideDuration)
            {
                _showTransitionDuration = showTransitionDuration;
                _hideTransitionDuration = hideTransitionDuration;
            }
            else
            {
                _showTransitionDuration = _customShowDuration;
                _hideTransitionDuration = _customHideDuration;
            }
            if(_executeBeforeChildTransitions)
            {
                UITransition [] childTransitions = GetComponentsInChildren<UITransition>(true);
                foreach (UITransition transition in childTransitions)
                {
                    if (transition != this)
                    {
                        transition._parentTransitionToWait = this;
                    }
                }
            }
        }

        protected abstract void OnShow(Action FinishCallback);
        protected abstract void OnHide(Action FinishCallback);
        public virtual void HideWithoutAnimation()
        {
            float tmp = _hideTransitionDuration;
            _hideTransitionDuration = 0;
            Hide(null);
            _hideTransitionDuration = tmp;
        }

        public void Show(Action FinishCallback)
        {
            _finishCallback = FinishCallback;
            if (_parentTransitionToWait?._finishCallback != null)
                _parentTransitionToWait._finishCallback += () => OnShow(TransitionCompleted);
            else
                OnShow(TransitionCompleted);
        }

        public void Hide(Action FinishCallback)
        {
            _finishCallback = FinishCallback;
            if (_parentTransitionToWait?._finishCallback != null)
                _parentTransitionToWait._finishCallback += () => OnHide(TransitionCompleted);
            else
                OnHide(TransitionCompleted) ;
        }

        public void Abort()
        {
            _finishCallback = null;
            OnAbort();
        }

        protected abstract void OnAbort();

        public abstract void Reset();

        private void TransitionCompleted()
        {
            Action tmpFinishCallback = _finishCallback;
            _finishCallback = null;
            tmpFinishCallback?.Invoke();
        }
    }
}