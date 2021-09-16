using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "CharacterConfig", menuName = "Configs/CharacterConfig", order = 0)]
    public class CharacterConfig : ScriptableObject
    {
        [SerializeField] private float _moveSpeed = 7.5f;
        [SerializeField, Range(0f, 1f)] private float _rotationSpeed = 0.4f;


        public float MoveSpeed => _moveSpeed;
        public float RotationSpeed => _rotationSpeed;
    }
}