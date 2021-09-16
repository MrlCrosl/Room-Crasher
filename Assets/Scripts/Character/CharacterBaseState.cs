using Interfces;
using UnityEngine;

public abstract class CharacterBaseState
{
   protected Character _character;
   public abstract void Enter();

   public virtual void Update()
   {
      
   }

   public virtual void FixedUpdate()
   {
      
   }

   public virtual void OnCollisionEnter(Collision collision)
   {
      if (collision.gameObject.TryGetComponent(out IExplosible explosibleObj))
      {
         
         if (explosibleObj.CanStun)
         {
            _character.SetState(_character.StunState);
            var stunState = _character.CurrentState as CharacterStunState;
            stunState.WaitForStunEnd(explosibleObj.StunDuration);
         }
         explosibleObj.Explode();
      }
   }
}
