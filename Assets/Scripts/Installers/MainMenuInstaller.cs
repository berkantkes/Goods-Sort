using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MainMenuInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<LoadingManager>().AsSingle();
        Container.Bind<MainMenu>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerProgressManager>().AsSingle();
    }
}
