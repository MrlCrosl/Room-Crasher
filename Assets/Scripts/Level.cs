using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public event Action<int> LevelCompleted;
    public event Action<int> ObstacleDestroyed;
    
    [SerializeField] private int _levelId;
    [SerializeField] private Transform _characterStartPoint;
    [SerializeField] private ExplosibleObject[] _destroyableObjects;
    
    private int _destroyedObjectsAmount;
    
    public int LevelId => _levelId;
    public Transform CharacterStartPoint => _characterStartPoint;
    public int ObjectsAmount { get; private set; }

    public void Initialize()
    {
        ObjectsAmount = _destroyableObjects.Length;
        foreach (var objects in _destroyableObjects)
        {
            objects.ObjectExplode += OnObjectExplode;
        }
    }

    private void OnObjectExplode()
    {
        _destroyedObjectsAmount++;
        ObstacleDestroyed?.Invoke(_destroyedObjectsAmount);
        if (_destroyedObjectsAmount >= ObjectsAmount)
        {
            LevelCompleted?.Invoke(LevelId);
        }
    }
}