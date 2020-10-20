using UnityEngine;
using Zenject;


public class VehicleScrollViewInstaller : MonoInstaller
{
    public GameObject VehicleButtonPrefab;

    public override void InstallBindings()
    {
        Container.BindFactory<VehicleButtonView, VehicleButtonView.Factory>().FromComponentInNewPrefab(VehicleButtonPrefab).NonLazy();
    }
}