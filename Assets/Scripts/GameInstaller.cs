using UnityEngine;
using Zenject;

public sealed class GameInstaller : MonoInstaller
{
    [SerializeField] private GameConfigSO _config;
    [SerializeField] private SceneReferences _refs;

    public override void InstallBindings()
    {
        Container.BindInstance(_config).AsSingle();
        Container.BindInstance(_refs).AsSingle();

        // view model
        Container.Bind<PaletteViewModel>().AsSingle();
        Container.Bind<TowerViewModel>().AsSingle();

        // services
        Container.Bind<TowerHeightLimitService>().AsSingle();
        Container.Bind<TowerPlacementService>().AsSingle();
        Container.Bind<EllipseHitTestService>().AsSingle();

        // controllers
        Container.Bind<DragDropController>().AsSingle();
        Container.Bind<PaletteController>().AsSingle();
        Container.Bind<TowerController>().AsSingle();
        Container.Bind<HoleController>().AsSingle();

        Container.BindInterfacesTo<GameBootstrapper>().AsSingle();
    }
}
