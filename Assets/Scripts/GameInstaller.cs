using UnityEngine;
using Zenject;

public sealed class GameInstaller : MonoInstaller
{
    [SerializeField] private GameConfigSO _gameConfig;
    [SerializeField] private LocalizationConfig _localizationConfig;
    [SerializeField] private SceneReferences _refs;

    public override void InstallBindings()
    {
        Container.BindInstance(_gameConfig).AsSingle();
        Container.BindInstance(_localizationConfig).AsSingle();
        Container.BindInstance(_refs).AsSingle();

        // view model
        Container.Bind<PaletteViewModel>().AsSingle();
        Container.Bind<TowerViewModel>().AsSingle();
        Container.Bind<MessageViewModel>().AsSingle();
        Container.Bind<HoleViewModel>().AsSingle();

        // services
        Container.Bind<TowerHeightLimitService>().AsSingle();
        Container.Bind<TowerPlacementService>().AsSingle();
        Container.Bind<EllipseHitTestService>().AsSingle();
        Container.Bind<LocalizationService>().AsSingle();

        // controllers
        Container.Bind<DragDropController>().AsSingle();
        Container.Bind<PaletteController>().AsSingle();
        Container.Bind<TowerController>().AsSingle();
        Container.Bind<HoleController>().AsSingle();
        Container.Bind<MessageController>().AsSingle();

        Container.BindInterfacesTo<GameBootstrapper>().AsSingle();
    }
}
