using UnityEngine;
using Zenject;

using model;


public class Container3DInstaller : MonoInstaller
{
    public GameObject Vehicle3DUrbanoPrefab;
    public GameObject Vehicle3DExtraUrbanoPrefab;

    public override void InstallBindings()
    {
        Container.BindFactory<Vehicle, Vehicle3D, Vehicle3D.Factory>()
            .WithId("UrbanoFactory")
            .FromMonoPoolableMemoryPool(x => 
                x.WithInitialSize(3)
                .FromComponentInNewPrefab(Vehicle3DUrbanoPrefab)
                .UnderTransformGroup("Vehicle3DUrbanoPool")
            );

        Container.BindFactory<Vehicle, Vehicle3D, Vehicle3D.Factory>()
            .WithId("ExtraUrbanoFactory")
            .FromMonoPoolableMemoryPool(x => 
                x.WithInitialSize(3)
                .FromComponentInNewPrefab(Vehicle3DExtraUrbanoPrefab)
                .UnderTransformGroup("Vehicle3DExtraUrbanoPool")
            );

        Container.BindInterfacesAndSelfTo<Container3DCoordinator>().AsSingle().NonLazy();
    }
}
