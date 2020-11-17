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

    private VehicleService _service;

    [Inject]
    private void Inject(Container3DCoordinator coordinator,
        VehicleService service)
    {
        _coordinator = coordinator;
        _service = service;
    }

    void Start()
    {
        //string filepath = @"Assets/StreamingAssets/Json/vehicle_list_full.json";
        //_service.path = filepath;
        _service.GetVehicles().Subscribe(x => _coordinator.AddVehicle3D(x));
    }
}
