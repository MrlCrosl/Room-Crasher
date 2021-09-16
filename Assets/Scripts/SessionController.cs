using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SessionController : MonoBehaviour
{
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private HUD _hud;
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private Character _character;

    private Level _currentLevel;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        InitializeUI();
    }

    private void InitializeUI()
    {
        _mainMenu.Initialize();
        _hud.Initialize();
        _mainMenu.Show();
    }

    public void StartSession()
    {
        _cameraController.EnableGamePlayCamera();
        SpawnNewLevel(1);
        _hud.Show();
    }

    private void OnLevelCompleted(int levelId)
    {
        _currentLevel.LevelCompleted -= OnLevelCompleted;
        _currentLevel.ObstacleDestroyed -= _hud.UpdateDestroyedObjects;

        if (GetLevelById(levelId + 1) != null)
        {
            Destroy(_currentLevel.gameObject);
            SpawnNewLevel(levelId + 1);
        }
    }

    private void SpawnNewLevel(int levelId)
    {
        _currentLevel = Instantiate(GetLevelById(levelId));
        _currentLevel.Initialize();
        _hud.Initialize(levelId, _currentLevel.ObjectsAmount);
        _currentLevel.LevelCompleted += OnLevelCompleted;
        _currentLevel.ObstacleDestroyed += _hud.UpdateDestroyedObjects;
        _character.transform.position = _currentLevel.CharacterStartPoint.position;
    }

    private Level GetLevelById(int id)
    {
        return Resources.Load<Level>($"Levels/Level_{id}");
    }
}