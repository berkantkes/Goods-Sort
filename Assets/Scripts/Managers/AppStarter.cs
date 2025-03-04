using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AppStarter : MonoBehaviour
{
    private LoadingManager _loadingManager;

    [Inject]
    public void Construct(LoadingManager loadingManager)
    {
        _loadingManager = loadingManager;
    }

    private void Awake()
    {
        _loadingManager.StartGame();
    }
}
