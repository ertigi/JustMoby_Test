using UnityEngine;
using Zenject;

public sealed class GameInstaller : MonoInstaller
{
    [SerializeField] private GameConfigSO _config;

    public override void InstallBindings()
    {
        Container.BindInstance(_config).AsSingle();

        Container.Bind<PaletteViewModel>().AsSingle();

        Container.Bind<PaletteController>().AsSingle();

        Container.BindInterfacesTo<GameBootstrapper>().AsSingle();
    }
}
