using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        // LoadingManager'ı global olarak bağla (bütün sahnelerde geçerli olacak)
        Container.Bind<LoadingManager>().FromComponentInHierarchy().AsSingle();
    }
}