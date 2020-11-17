using UnityEngine;
using Zenject;

using model;


public class VehicleScrollViewInstaller : MonoInstaller
{
    public GameObject VehicleButtonPrefab;
    public GameObject VehiclePanelPrefab;
    public VehicleSettings Settings;
    public VehicleGPSSettings GPSSettings;

    public override void InstallBindings()
    {
        Container.BindFactory<Vehicle, VehicleButtonView, VehicleButtonView.Factory>()
            .FromMonoPoolableMemoryPool(x => 
                x.WithInitialSize(50)
                .FromComponentInNewPrefab(VehicleButtonPrefab)
                .UnderTransformGroup("VehicleButtonPool")
            );

        Container.BindFactory<Vehicle, VehicleDetailsPanelView, VehicleDetailsPanelView.Factory>()
            .FromMonoPoolableMemoryPool(x => 
                x.WithInitialSize(3)
                .FromComponentInNewPrefab(VehiclePanelPrefab)
                .UnderTransformGroup("VehiclePanelPool")
            );

        Container.BindInstance(Settings).AsSingle().NonLazy();
    
        Container.BindInstance(GPSSettings).AsSingle().NonLazy();

        Container.BindInterfacesTo<VehiclePanelsCoordinator>().AsSingle().NonLazy();
    }
}