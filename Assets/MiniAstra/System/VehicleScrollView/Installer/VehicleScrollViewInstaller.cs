using UnityEngine;
using Zenject;

using model;


public class VehicleScrollViewInstaller : MonoInstaller
{
    public GameObject VehicleButtonPrefab;
    public GameObject VehiclePanelPrefab;

    public VehicleSettings Settings;

    public override void InstallBindings()
    {
        Container.BindFactory<Vehicle, VehicleButtonView, VehicleButtonView.Factory>().FromMonoPoolableMemoryPool(x => 
            x.WithInitialSize(50)
            .FromComponentInNewPrefab(VehicleButtonPrefab)
            .UnderTransformGroup("VehicleButtonPool")
        );

        Container.BindInstance(Settings).AsSingle().NonLazy();

        Container.BindFactory<Vehicle, VehicleDetailsPanelView, VehicleDetailsPanelView.Factory>().FromMonoPoolableMemoryPool(x => 
            x.WithInitialSize(3)
            .FromComponentInNewPrefab(VehiclePanelPrefab)
            .UnderTransformGroup("VehicleButtonPool")
        );

        SignalBusInstaller.Install(Container);

        Container.DeclareSignal<PanelOpenSignal>();
        Container.DeclareSignal<PanelCloseSignal>();

        Container.BindInterfacesTo<VehiclePanelsCoordinator>().AsSingle();
    }
}