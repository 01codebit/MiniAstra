using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;
using UniRx;

using model;
using service;


public class Container3DFacade : MonoBehaviour
{
    private Container3DCoordinator _coordinator;

    private IAsstraService _service;
    private IAsstraService _feeder;

    [Inject]
    private void Inject(Container3DCoordinator coordinator,
          VehicleService service,
          ContinuosFeeder feeder)
    {
        _coordinator = coordinator;
        _service = service;
        _feeder = feeder;
    }

    void Start()
    {
        //string filepath = @"Assets/StreamingAssets/Json/vehicle_list_full.json";
        //_service.path = filepath;

        //_service.GetVehicles().Subscribe(x => _coordinator.AddVehicle3D(x));
        _feeder.GetVehicles()
            .SubscribeOn(Scheduler.ThreadPool)
            .ObserveOnMainThread()
            .Subscribe(x => _coordinator.AddVehicle3D(x));            
    }
}
