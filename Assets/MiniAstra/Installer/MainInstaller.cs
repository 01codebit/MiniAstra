using UnityEngine;
using Zenject;

using model;
using service;
using jsonloader;

using TMPro;

public class MainInstaller : MonoInstaller<MainInstaller>
{
    public VehicleSettings Settings;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<JsonLoader<Vehicle>>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<VehicleService>().AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<ContinuosFeeder>().AsSingle().NonLazy();

        Container.BindInstance(Settings).AsSingle().NonLazy();

        SignalBusInstaller.Install(Container);

        Container.DeclareSignal<PanelOpenSignal>();
        Container.DeclareSignal<PanelCloseSignal>();
    }
}
