using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class HUD : UIGroup
{
  
    [SerializeField] private TextMeshProUGUI _level;
    [SerializeField] private TextMeshProUGUI _destroyedObjects;

    private int _objectsAmount;
    
    public void Initialize(int level, int objectsAmount)
    {
        _level.text = $"Level {level + 1}";
        _destroyedObjects.text = $"0/{objectsAmount}";
        _objectsAmount = objectsAmount;
    }

    public void UpdateDestroyedObjects(int destroyedObjects)
    {
        _destroyedObjects.DOKill();
        _destroyedObjects.transform.localScale = Vector3.one;
        _destroyedObjects.transform.DOScale(1.3f, 0.1f).SetLoops(2, LoopType.Yoyo);
        _destroyedObjects.text = $"{destroyedObjects}/{_objectsAmount}";
    }

    public override void Reset()
    {
        
    }
}
