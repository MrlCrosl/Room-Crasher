using UnityEngine;
using System;
using DefaultNamespace;
using Interfces;

public class ExplosibleObject : MonoBehaviour, IExplosible
{
    public event Action ObjectExplode;
    
    [SerializeField] private GameObject _solidGameObject;
    [SerializeField] private GameObject _splitGameObject;
    [SerializeField] private ExplosibleObjectConfig _config;
    
    private SplitObjectFacade _splitObjectFacade;
    private BoxCollider _collider;
    private Vector3 _particlesOffset = new Vector3(0, 0.5f, 0);
    public bool IsDestroyed { get; protected  set ; }
    public float StunDuration { get => _config.StunDuration;}
    public bool CanStun { get => _config.CanStun; } 

    private void Start()
    {
        InitCollider();
        _splitObjectFacade = new SplitObjectFacade(new SplitObject(_splitGameObject.transform, _config));
    }
    
    public void Explode()
    {
        if(IsDestroyed)
            return;
        
        ObjectExplode?.Invoke();
        IsDestroyed = true;
        _collider.enabled = false;
        Instantiate(Resources.Load(_config.ExplosionParticlesPath), transform.position + _particlesOffset, Quaternion.identity);
        _solidGameObject.SetActive(false);
        _splitObjectFacade.ExplodeObject(DisableGameObject);
    }

    private void DisableGameObject()
    {
        Destroy(gameObject);
    }
    
    private void InitCollider()
    {
        _collider = gameObject.AddComponent<BoxCollider>();
        Bounds bounds = BoundsHelper.Calculate(transform, _splitGameObject.GetComponentsInChildren<Renderer>());
        _collider.center = bounds.center;
        _collider.size = bounds.size;
    }
}
