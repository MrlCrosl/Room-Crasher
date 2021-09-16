using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DG.Tweening;
using Interfces;
using UnityEngine;

public class ExplosibleObjectSolid : MonoBehaviour, IExplosible
{
    public event Action ObjectExplode;

    [SerializeField] private ExplosibleObjectConfig _config;
    [SerializeField] private Rigidbody _rigidbody;
    public bool IsDestroyed { get; protected set; }
    private Vector3 _particlesOffset = new Vector3(0, 0.5f, 0);
    public float StunDuration => _config.StunDuration;

    public bool CanStun
    {
        get
        {
            if (!IsDestroyed)
                return _config.CanStun;
            return false;
        }
    }

    public void Explode()
    {
        if (IsDestroyed)
            return;
        ObjectExplode?.Invoke();
        IsDestroyed = true;
        Instantiate(Resources.Load(_config.ExplosionParticlesPath),transform.position + _particlesOffset, Quaternion.identity);
        AddForceToRb();
        StartCoroutine(HideObject());
    }

    private void AddForceToRb()
    {
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;
        _rigidbody.AddForce(Vector3.up * _config.ExpansionForce, ForceMode.Impulse);
    }

    private IEnumerator HideObject()
    {
        yield return new WaitForSeconds(_config.DelayBeforeDisappearing);
        transform.DOScale(Vector3.zero, _config.DisappearingDuration).OnComplete(()=>Destroy(gameObject));
    }
}