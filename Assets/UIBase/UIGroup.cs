using System;
using System.Collections.Generic;
using BasicUI.UI.Core;
using UnityEngine;

public interface IUIGroup
{
    event Action GroupHidden;
    event Action GroupShown;
    bool Visible { get; }
    bool Initialized { get; }
    bool TransitionsActive { get; }
    bool Show();
    bool Hide();
    void Initialize();
    void SetOrUpdateMainTransition(UITransitionBuilder transitionBuilder);
}

public abstract class UIGroup : MonoBehaviour, IUIGroup
{ 
    [SerializeField]
    protected Canvas _canvas;
    [SerializeField]
    private float _hideTransitionDuration = 0f;
    [SerializeField]
    private float _showTransitionDuration = 0f;
    
    private int _numOfActiveTransitions;
    private UITransition _mainTransition;
    private List<UITransition> _uiTransitions = new List<UITransition>();
    private IUIGroup _iuiGroupImplementation;


    public bool Initialized { get; private set; }

    public event Action GroupHidden;
    public event Action GroupShown;
    public bool Visible {get; private set;}

        public bool TransitionsActive { get => _numOfActiveTransitions > 0; }

        public Canvas Canvas => _canvas;

        public abstract void Reset();

        public void Initialize()
        {
            if (Initialized)
            {
                return;
            }
            OnInitialize();
            _mainTransition = GetComponent<UITransition>();
        }

        protected virtual void OnInitialize()
        {
            
            _uiTransitions.AddRange(transform.GetComponentsInChildren<UITransition>(true));
            foreach (UITransition transition in _uiTransitions)
            {
                transition.Initialize(_showTransitionDuration, _hideTransitionDuration);
            }
            Initialized = true;
            
                foreach (UITransition transition in _uiTransitions)
                {
                    if(transition.enabled)
                        transition.HideWithoutAnimation();
                }
                if (_canvas != null)
                {
                    _canvas.enabled = false;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            
        }

        [ContextMenu("Show")]
        public bool Show()
        {
            if (Visible)
            {
                Debug.LogWarning("Cannot show, uigroup is already on the screen");
                return false;
            }
            Visible = true;
            if (_numOfActiveTransitions != 0)
            {
              
                for (int i = 0; i < _uiTransitions.Count; i++)
                {
                    _uiTransitions[i].Abort();
                }
                GroupHidden = null;
                _numOfActiveTransitions = 0;
            }
        
            BeforeShowInternal();
            _numOfActiveTransitions = 0;
            if (_uiTransitions.Count != 0)
            {
                foreach (UITransition transition in _uiTransitions)
                {
                    if (transition.enabled)
                    {
                        transition.Show(OnShowTransitionFinished);
                        _numOfActiveTransitions++;
                    }
                }
            }
            else
            {
                AfterShowInternal();
            }
            return true;
        }

        [ContextMenu("Hide")]
        public bool Hide()
        {
            if (!Visible)
            {
                Debug.LogWarning("Cannot hide, uigroup is not visilble");
                return false;
            }
            Visible = false;
            if (_numOfActiveTransitions != 0)
            {
                for (int i = 0; i < _uiTransitions.Count; i++)
                {
                    _uiTransitions[i].Abort();
                }
                _numOfActiveTransitions = 0;
            }

            BeforeHide();
            _numOfActiveTransitions = 0;
            if (_uiTransitions.Count != 0)
            {
                foreach (UITransition transition in _uiTransitions)
                {
                    if (transition.enabled)
                    {
                        transition.Hide(OnHideTransitionFinished);
                        _numOfActiveTransitions++;
                    }else
                    {
                        Debug.Log("skip transition on " + transition.name);
                    }
                }
            }
            else
            {
                AfterHideInternal();
            }
            return true;
        }
        
        public void SetOrUpdateMainTransition(UITransitionBuilder transitionBuilder)
        {
            if(_mainTransition)
            {
                UITransition newTransition = transitionBuilder.AttachToObject(gameObject);
                if(_mainTransition != null)
                    Destroy(_mainTransition);
                _mainTransition = newTransition;
            }
        }
        

        protected virtual void BeforeHide() { }

        protected virtual void AfterHide() { }

        protected virtual void BeforeShow(){}

        protected virtual void AfterShow(){}
        

        private void AfterShowInternal() 
        {
            GroupShown?.Invoke();
            AfterShow();
        }

        private void BeforeShowInternal() 
        {
            if (_canvas != null)
            {
                _canvas.enabled = true;
            }
            else
            {
                gameObject.SetActive(true);
            }
            BeforeShow(); 
        }

        private void AfterHideInternal() 
        {
            if (_canvas != null)
            {
                _canvas.enabled = false;
            }
            else
            {
                gameObject.SetActive(false);
            }
            AfterHide();
            Reset();
        }

        private void OnShowTransitionFinished()
        {
            _numOfActiveTransitions--;
            if(_numOfActiveTransitions == 0)
            {
                AfterShowInternal();
            }
            if (_numOfActiveTransitions < 0)
            {
                _numOfActiveTransitions = 0;
                Debug.LogError("Something went wrong, finished more transitions than were started");
            }
        }

        private void OnHideTransitionFinished()
        {
            _numOfActiveTransitions--;
            if (_numOfActiveTransitions == 0)
            {
                AfterHideInternal();
                GroupHidden?.Invoke();
            }
            if (_numOfActiveTransitions < 0)
            {
                _numOfActiveTransitions = 0;
                Debug.LogError("Something went wrong, finished more transitions than were started");
            }
        }


    }

