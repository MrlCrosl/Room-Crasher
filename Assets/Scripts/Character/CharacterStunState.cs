using System;
using System.Threading.Tasks;
using UnityEngine;

public class CharacterStunState : CharacterBaseState
{
    public CharacterStunState(Character character)
    {
        _character = character;
    }
    
    public override void Enter()
    {
        _character.Animator.SetTrigger(GlobalConstants.AnimKeyInjured);
        ResetCharacterVelocity();
        _character.StunParticles.Play();
    }
  
    
    public async void WaitForStunEnd(float stunDuration)
    {
        await Task.Delay((int) TimeSpan.FromSeconds(stunDuration).TotalMilliseconds);
      _character.SetState(_character.IdleState);
      _character.StunParticles.Stop();
    }
    
    private void ResetCharacterVelocity()
    {
        _character.Rigidbody.velocity = Vector3.zero;
    }
    
}
