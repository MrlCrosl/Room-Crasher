
using UnityEngine;

namespace BasicUI.UI.Core
{
    public interface UITransitionBuilder
    {
        UITransition AttachToObject(GameObject objectToAttach);
    }

    public class UITransitionBuilder<T> where T : UITransition
    {
        private bool _copyValuesFromOld;
        private bool _overrideDuration;
        private bool _executeBeforeChild;
        private float _customShowDuration;
        private float _customHideDuration;
        
        private T _transition;

        public UITransitionBuilder(bool copyValuesFromOldTransitionIfExists)
        {
            _copyValuesFromOld = copyValuesFromOldTransitionIfExists;
        }

        public UITransition AttachToObject(GameObject objectToAttach)
        {
            T oldTransition = objectToAttach.GetComponent<T>();
            T newTransition = objectToAttach.AddComponent<T>();
            if(_copyValuesFromOld)
            {
                CopyValuesFromOldTransition(oldTransition, newTransition);
            }
            return newTransition;
        }

        protected virtual void CopyValuesFromOldTransition(T oldTransition, T newTransition)
        {
            newTransition._overrideDuration = oldTransition._overrideDuration;
            newTransition._customShowDuration = oldTransition._customShowDuration;
            newTransition._customHideDuration = oldTransition._customHideDuration;
            newTransition._executeBeforeChildTransitions = oldTransition._executeBeforeChildTransitions;
        }
        
        protected virtual void SetValues(T newTransition)
        {
            newTransition._overrideDuration = _overrideDuration;
            newTransition._executeBeforeChildTransitions = _executeBeforeChild;
            newTransition._customShowDuration = _customShowDuration;
            newTransition._customHideDuration = _customHideDuration;
        }

        public UITransitionBuilder<T> SetOverrideDuration(bool overrideDuration)
        {
            _overrideDuration = overrideDuration;
            return this;
        }

        public UITransitionBuilder<T> SetCustomShowDuration(float customShowDuration)
        {
            _customShowDuration = customShowDuration;
            return this;
        }

        public UITransitionBuilder<T> SetCustomHideDuration(float customHideDuration)
        {
            _customHideDuration = customHideDuration;
            return this;
        }

        public UITransitionBuilder<T> SetExecuteBeforeChildTransitions(bool executeBeforeChild)
        {
            _executeBeforeChild = executeBeforeChild;
            return this;
        }
    }
}
