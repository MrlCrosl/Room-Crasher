using System;

namespace Interfces
{
    public interface IExplosible
    { 
        event Action ObjectExplode; 
        bool IsDestroyed { get; }
        float StunDuration { get; }
        bool CanStun { get; }
        void Explode();
    }
}