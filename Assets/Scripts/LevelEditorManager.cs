using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorManager : MonoBehaviour
{
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private ObjectPoolManager _objectPoolManager;
    [SerializeField] private SelectAndDragItem _selectAndDragItem;
    [SerializeField] private Camera _camera;

    private bool _sceneIsReady = false;
    private void Awake()
    {
        _objectPoolManager.Initialize();
        CreateLevel(1);
    }

    public void SetLevel(int levelNumber)
    {
        CreateLevel(levelNumber);
    }

    private void CreateLevel(int levelNumber)
    {
        //_selectAndDragItem.Initialize(_camera);
        _levelManager.Initialize(levelNumber);
    }
}