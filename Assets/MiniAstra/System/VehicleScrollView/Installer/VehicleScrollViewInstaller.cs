using UnityEngine;
using Zenject;

using model;


public class VehicleScrollViewInstaller : MonoInstaller
{
    public GameObject VehicleButtonPrefab;

    public VehicleSettings Settings;

    public override void InstallBindings()
    {
        //Container.BindFactory<VehicleButtonView, VehicleButtonView.Factory>().FromComponentInNewPrefab(VehicleButtonPrefab).NonLazy();

        Container.BindFactory<Vehicle, VehicleButtonView, VehicleButtonView.Factory>().FromMonoPoolableMemoryPool(x => 
            x.WithInitialSize(50)
            .FromComponentInNewPrefab(VehicleButtonPrefab)
            .UnderTransformGroup("VehicleButtonPool")
        );

        Container.BindInstance(Settings).AsSingle().NonLazy();


        // Container.BindMemoryPool<VehicleButtonView, VehicleButtonView.Pool>().WithInitialSize(10).FromComponentInNewPrefab(VehicleButtonPrefab).NonLazy();
    }
}