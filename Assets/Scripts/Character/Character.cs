using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.PlayerLoop;


[RequireComponent(typeof(Rigidbody))]
public class Character :MonoBehaviour, IMovableObject 
{
    [SerializeField] private Joystick _joystick;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _stunParticles;
    [SerializeField] private CharacterConfig _config;

    private CharacterBaseState _currentState;
    private CharacterIdleState _idleState;
    private CharacterWalkingState _walkingState;
    private CharacterStunState _stunState;

    
    public Animator Animator => _animator;

    public Rigidbody Rigidbody => _rigidbody;

    public Joystick Joystick => _joystick;

    public CharacterIdleState IdleState => _idleState;

    public CharacterWalkingState WalkingState => _walkingState;

    public CharacterStunState StunState => _stunState;

    public CharacterBaseState CurrentState => _currentState;

    public ParticleSystem StunParticles => _stunParticles;

    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        InitializeStates();
        SetState(_idleState);
    }
    
    public void SetState(CharacterBaseState state)
    {
        _currentState = state;
        _currentState.Enter();
    }

    public void Move(Vector3 direction)
    {
        _rigidbody.velocity = SwapYAndZAxis(direction) * _config.MoveSpeed;
    }

    public void Rotate(Vector3 direction)
    {
        _rigidbody.rotation = Quaternion.Slerp(_rigidbody.rotation, Quaternion.LookRotation(SwapYAndZAxis(direction).normalized), _config.RotationSpeed);
    }

    private void InitializeStates()
    {
        _idleState = new CharacterIdleState(this);
        _walkingState = new CharacterWalkingState(this);
        _stunState = new CharacterStunState(this);
    }
    
    private void Update()
    {
        _currentState.Update();
    }

    private void FixedUpdate()
    {
        _currentState.FixedUpdate();
    }

    private Vector3 SwapYAndZAxis(Vector2 vector)
    {
        return new Vector3(vector.x, 0, vector.y);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _currentState.OnCollisionEnter(collision);
    }
}