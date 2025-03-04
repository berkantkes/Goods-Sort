using Zenject;
using UnityEngine;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private AllLevelsData allLevelsData;
    [SerializeField] private PoolData poolData;
    [SerializeField] private ShelfController shelfPrefab;
    [SerializeField] private Camera mainCamera;

    public override void InstallBindings()
    {
        Container.Bind<ObjectPoolManager>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<GameManager>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<LevelManager>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerProgressManager>().AsSingle();
        Container.Bind<TimerController>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<EventManager>().AsSingle();
        Container.Bind<LoadingManager>().AsSingle();
        Container.Bind<GameUiManager>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<SelectAndDragItem>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<StarController>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<ComboController>().FromComponentInHierarchy().AsSingle().NonLazy();

        Container.BindInstance(allLevelsData);
        Container.BindInstance(poolData);
        
        Container.Bind<ShelfController>().FromInstance(shelfPrefab).AsSingle();
        Container.Bind<Camera>().FromInstance(mainCamera).AsSingle();
    }
}