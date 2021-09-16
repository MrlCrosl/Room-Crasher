using UnityEngine;

public  class CharacterIdleState : CharacterBaseState
{
    public CharacterIdleState(Character character)
    {
        _character = character;
    }
    public override void Enter( )
    {
        _character.Animator.SetFloat(GlobalConstants.AnimKeyForSpeed,0);
        _character.Animator.SetTrigger(GlobalConstants.AnimKeyWalk);
        ResetCharacterVelocity();
    }

    public override void Update( )
    {
        if (_character.Joystick.Direction.magnitude > 0)
            _character.SetState(_character.WalkingState);
    }
    
    
    private void ResetCharacterVelocity()
    {
        _character.Rigidbody.velocity = Vector3.zero;
    }
}
