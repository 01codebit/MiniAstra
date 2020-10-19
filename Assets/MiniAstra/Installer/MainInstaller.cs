using UnityEngine;
using Zenject;

using model;
using service;
using jsonloader;


public class MainInstaller : MonoInstaller<MainInstaller>
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<VehicleService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<JsonLoader<Vehicle>>().AsSingle().NonLazy();
    }
}
