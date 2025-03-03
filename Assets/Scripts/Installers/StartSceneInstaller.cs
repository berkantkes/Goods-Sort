using UnityEngine;
using Zenject;

public class StartSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<LoadingManager>().AsSingle();
        Container.Bind<AppStarter>().FromComponentInHierarchy().AsSingle().NonLazy();
    }
}