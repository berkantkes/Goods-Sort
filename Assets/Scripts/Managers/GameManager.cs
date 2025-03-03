using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    private LevelManager _levelManager;
    private ObjectPoolManager _objectPoolManager;

    private bool _sceneIsReady = false;

    [Inject]
    public void Construct(LevelManager levelManager, ObjectPoolManager objectPoolManager)
    {
        _levelManager = levelManager;
        _objectPoolManager = objectPoolManager;
    }

    private void Awake()
    {
        _objectPoolManager.Initialize();
        _levelManager.Initialize();
        _sceneIsReady = true;
    }

    public bool GetSceneIsReady()
    {
        return _sceneIsReady;
    }
}