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
    private SignalBus _signalBus;

    private List<VehicleButtonView> _buttons;


    [Inject]
    private void Inject(VehicleService service, 
        [Inject (Id = "Content")] RectTransform transform,
        VehicleButtonView.Factory buttonFactory,
        SignalBus signalBus)
    {
        _service = service;
        _contentTransform = transform;
        _buttonFactory = buttonFactory;
        _signalBus = signalBus;
    }
    
    void Start()
    {
        //string filepath = @"Assets/StreamingAssets/Json/vehicle_list_full.json";
        //_service.path = filepath;

        _buttons = new List<VehicleButtonView>();

        _service.GetVehicles().Subscribe(x => OnGetVehicle(x));

        _signalBus.GetStream<SearchTextSignal>()
            .SubscribeOn(Scheduler.ThreadPool)
            .ObserveOnMainThread()
            .Subscribe((s) => OnSearchTextSignal(s.Text));
    }

    void OnGetVehicle(Vehicle v)
    {
        VehicleButtonView newButton = _buttonFactory.Create(v);
        newButton.transform.SetParent(_contentTransform);

        _buttons.Add(newButton);
    }

    void OnSearchTextSignal(string text)
    {

        Debug.Log("[VehicleScrollView.OnSearchTextSignal] " + text);
        Debug.Log("[VehicleScrollView.OnSearchTextSignal] _buttons.Count: " + _buttons.Count);
        foreach(var x in _buttons)
        {
            if(text=="")
                x.GameObject.SetActive(true);
            else if(text=="x")                
                x.GameObject.SetActive(false);
            else
            {
                if(x.GetDestination()!=null)
                {
                    x.GameObject.SetActive(x.GetDestination() == "" ? true : x.GetDestination().StartsWith(text));
                }
            }
        }
    }
}
