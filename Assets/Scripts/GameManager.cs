using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private ObjectPoolManager _objectPoolManager;
    [SerializeField] private SelectAndDragItem _selectAndDragItem;
    [SerializeField] private Camera _camera;

    private bool _sceneIsReady = false;
    private void Awake()
    {
        _selectAndDragItem.Initialize(_camera);
        _objectPoolManager.Initialize();
        _levelManager.Initialize();
        _sceneIsReady = true;
    }

    public bool GetSceneIsReady()
    {
        return _sceneIsReady;
    }
}
