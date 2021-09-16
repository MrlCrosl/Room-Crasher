using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "ExplosibleObjectConfig", menuName = "Configs/ExplosibleObjectConfig", order = 0)]
    public class ExplosibleObjectConfig : ScriptableObject
    {
        [SerializeField] private float _disappearingDuration = 0.5f;
        [SerializeField] private float _delayBeforeDisappearing = 2f;
        [SerializeField] private float _expansionForce = 2f;
        [SerializeField] private float _stunDuration = 2f;
        [SerializeField] private bool _canStun;
        [SerializeField] private LayerMask _propPartsLayer;
        [SerializeField] private string _explosionParticlesPath;

        public float DisappearingDuration => _disappearingDuration;

        public float DelayBeforeDisappearing => _delayBeforeDisappearing;

        public float ExpansionForce => _expansionForce;

        public LayerMask PropPartsLayer => _propPartsLayer;

        public float StunDuration => CanStun ? _stunDuration : 0;

        public bool CanStun => _canStun;

        public string ExplosionParticlesPath => _explosionParticlesPath;
    }
}