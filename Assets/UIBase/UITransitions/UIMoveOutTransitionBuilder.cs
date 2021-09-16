
using BasicUI.UI;
using BasicUI.UI.Core;

namespace BasicUI
{
    public class UIMoveOutTransitionBuilder : UITransitionBuilder<UIMoveOutTransition>
    {
        private MoveOutDirection _moveOutDirection;

        public UIMoveOutTransitionBuilder(bool copyValuesFromOldTransitionIfExists) : base(copyValuesFromOldTransitionIfExists)
        {
        }

        protected override void CopyValuesFromOldTransition(UIMoveOutTransition oldTransition, UIMoveOutTransition newTransition)
        {
            base.CopyValuesFromOldTransition(oldTransition, newTransition);
            newTransition._moveOutDirection = oldTransition._moveOutDirection;
        }

        protected override void SetValues(UIMoveOutTransition newTransition)
        {
            base.SetValues(newTransition);
            newTransition._moveOutDirection = _moveOutDirection;
        }

        public UIMoveOutTransitionBuilder SetMoveOutDirection(MoveOutDirection moveOutDirection)
        {
            _moveOutDirection = moveOutDirection;
            return this;
        }
    }
}
