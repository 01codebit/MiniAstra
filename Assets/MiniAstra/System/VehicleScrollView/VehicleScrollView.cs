using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Zenject;

using model;
using service;


public class VehicleScrollView : MonoBehaviour
{
    private RectTransform _contentTransform;

    private VehicleService _service;
    private VehicleButtonView.Factory _buttonFactory;

    [Inject]
    private void Inject(VehicleService service, 
        [Inject (Id = "Content")] RectTransform transform,
        VehicleButtonView.Factory buttonFactory)
    {
        _service = service;
        _contentTransform = transform;
        _buttonFactory = buttonFactory;
    }
    
    void Start()
    {
        string filepath = @"Assets/StreamingAssets/Json/vehicle_list_full.json";
        _service.path = filepath;
        _service.GetVehicles().Subscribe(x => OnGetVehicle(x));
    }

    void OnGetVehicle(Vehicle v)
    {
        VehicleButtonView newButton = _buttonFactory.Create(v);
        newButton.transform.SetParent(_contentTransform);
    }
}
