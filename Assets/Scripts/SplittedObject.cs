using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DefaultNamespace;
using DG.Tweening;
using UnityEngine;

public class SplitObject
{
    public event Action AllPartsDisappeared;
    private List<Rigidbody> _objectParts = new List<Rigidbody>();
    private GameObject _splitObject;
    private readonly float _disappearingDuration;
    private readonly float _disappearingDelay;
    private readonly float _expansionForce;

    private Task _scaleDownPartsTask;
    private int _disappearedPartsAmount;
    public SplitObject(Transform splitObjectParent, ExplosibleObjectConfig config)
    {
        _splitObject = splitObjectParent.gameObject;
        _expansionForce = config.ExpansionForce;
        _disappearingDelay = config.DelayBeforeDisappearing;
        _disappearingDuration = config.DisappearingDuration;
        
        foreach (var objectPart in splitObjectParent.GetComponentsInChildren<Renderer>())
        {
            var rb = objectPart.gameObject.AddComponent<Rigidbody>();
            objectPart.gameObject.AddComponent<BoxCollider>();
            objectPart.gameObject.layer = (int) Mathf.Log(config.PropPartsLayer.value, 2);;
            rb.mass = 0.5f;
            _objectParts.Add(rb);
        }
        
        splitObjectParent.gameObject.SetActive(false);
    }

    public void EnableGameObject()
    {
        _splitObject.gameObject.SetActive(true);
    }
    public void AddForceToParts()
    {
        foreach (var part in _objectParts)
            part.AddForce(Vector2.up * _expansionForce, ForceMode.Impulse);
    }
    
    public async void ScaleDownParts()
    {
        await Task.Delay((int) TimeSpan.FromSeconds(_disappearingDelay).TotalMilliseconds);
        foreach (var part in _objectParts)
            if (part != null)
                part.transform.DOScale(Vector3.zero, _disappearingDuration).OnComplete(HandleDisappearedPart);

    }

    private void HandleDisappearedPart()
    {
        _disappearedPartsAmount++;
        if (_disappearedPartsAmount == _objectParts.Count)
        {
            AllPartsDisappeared?.Invoke();
        }
    }
}
