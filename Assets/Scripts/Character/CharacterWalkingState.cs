using UnityEngine;

public class CharacterWalkingState : CharacterBaseState
{
    public CharacterWalkingState(Character character)
    {
        _character = character;
    }
    public override void Enter( )
    {
        _character.Animator.SetTrigger(GlobalConstants.AnimKeyWalk);
    }

    public override void Update( )
    {
        if (_character.Joystick.Direction.magnitude <= 0)
        {
            _character.SetState(_character.IdleState);
            return;
        }
            
        _character.Animator.SetFloat(GlobalConstants.AnimKeyForSpeed, _character.Joystick.Direction.magnitude);
    }

    public override void FixedUpdate( )
    {
        if (_character.Joystick.Direction != Vector2.zero)
            _character.Rotate(_character.Joystick.Direction);

        _character.Move(_character.Joystick.Direction);
    }


    
}
