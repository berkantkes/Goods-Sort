using Zenject;
using UnityEngine;

public class LevelEditorInstaller : MonoInstaller
{
    [SerializeField] private AllLevelsData allLevelsData;
    [SerializeField] private PoolData poolData;
    [SerializeField] private ShelfController shelfPrefab;
    [SerializeField] private Camera mainCamera;

    public override void InstallBindings()
    {
        Container.Bind<ObjectPoolManager>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<LevelEditorManager>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<LevelManager>().FromComponentInHierarchy().AsSingle().NonLazy();
        
        Container.Bind<EventManager>().AsSingle();
        Container.Bind<PlayerProgressManager>().AsSingle();

        Container.BindInstance(allLevelsData);
        Container.BindInstance(poolData);
        
        Container.Bind<ShelfController>().FromInstance(shelfPrefab).AsSingle();
        Container.Bind<Camera>().FromInstance(mainCamera).AsSingle();
    }
}