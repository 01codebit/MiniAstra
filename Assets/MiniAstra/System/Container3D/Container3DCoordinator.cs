using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Zenject;
using UnityEngine;

using System;
using System.Collections.Specialized;
using System.Linq;
using UniRx;
using TMPro;

using common;
using model;


public class Container3DCoordinator : IInitializable
{
    private SignalBus _signalBus;

    private Vehicle3D.Factory _urbanoFactory;
    private Vehicle3D.Factory _extraurbanoFactory;

    private Directory<Vehicle3D> _vehicles3D;
    private Container3DView _container3dView;


    [Inject]
    private void Inject(SignalBus signalBus,
        [Inject (Id = "UrbanoFactory")] Vehicle3D.Factory urbanoFactory,
        [Inject (Id = "ExtraUrbanoFactory")] Vehicle3D.Factory extraurbanoFactory,
        Container3DView container3dView)
    {
        _signalBus = signalBus;

        _urbanoFactory = urbanoFactory;
        _extraurbanoFactory = extraurbanoFactory;
        _container3dView = container3dView;
    }


    public void Initialize()
    {
        /*
        _signalBus.GetStream<PanelOpenSignal>()
            .SubscribeOn(Scheduler.ThreadPool)
            .ObserveOnMainThread()
            .Subscribe((s) => OpenPanel(s));

        _signalBus.GetStream<PanelCloseSignal>()
            .SubscribeOn(Scheduler.ThreadPool)
            .ObserveOnMainThread()
            .Subscribe((s) => OnPanelCloseSignal(s));
        */

        _signalBus.GetStream<SearchTextSignal>()
            .SubscribeOn(Scheduler.ThreadPool)
            .ObserveOnMainThread()
            .Subscribe((s) => OnSearchTextSignal(s.Text));


        _vehicles3D = new Directory<Vehicle3D>();
        _vehicles3D.Initialize();
    }

    public void OnPanelCloseSignal(PanelCloseSignal signal)
    {
        var sender = signal.Panel;
        var vid = sender.GetVehicleId();

        Vehicle3D vehicle3d = _vehicles3D.GetItem(vid);
        _vehicles3D.RemoveItem(vid);
        vehicle3d?.Dispose();
    }

    void OpenPanel(PanelOpenSignal signal)
    {
        var v = signal.SelectedVehicle;

        Vehicle3D result = null;
        result = _vehicles3D.GetItem(v.Id);

        if (result != null)
        {
            Debug.Log("panello trovato");
            _vehicles3D.RemoveItem(v.Id);
            result.Dispose();
        }
        else
        {
            AddVehicle3D(v);
        }
    }

    void OnSearchTextSignal(string text)
    {
        Debug.Log("[Container3DCoordinator.OnSearchTextSignal] " + text);
        foreach(var x in _vehicles3D.GetItems())
        {
            if(text=="")
                x.gameObject.SetActive(true);
            else
                if(x.GetDestination()!=null)
                    x.gameObject.SetActive(x.GetDestination() == "" ? false : x.GetDestination().StartsWith(text));
        }
    }

    public void AddVehicle3D(Vehicle v)
    {
        if(v!=null)
        {
            if(v.Avm && v.Location!=null && v.Location.Coordinates!=null)
            {

                var vv = _vehicles3D.GetItem(v.Id);
                if(vv==null)
                {
                    Vehicle3D vehicle3D = null;
                    if(v.CodiceFamiglia==model.CodiceFamigliaEnum.EXTRAURBANO)
                        vehicle3D = _extraurbanoFactory.Create(v);
                    else
                        vehicle3D = _urbanoFactory.Create(v);

                    SetVehicle3DTransform(vehicle3D, v);

                    _vehicles3D.AddItem(v.Id, vehicle3D);
                }
                else 
                {
                    SetVehicle3DTransform(vv, v);
                }
            }
        }
    }


    void SetVehicle3DTransform(Vehicle3D vehicle3D, Vehicle v)
    {
        vehicle3D.gameObject.transform.SetParent(_container3dView.GetTransform());

        var latitude = v.Location.Coordinates[0];
        var longitude = v.Location.Coordinates[1];

        float x = (float)(latitude * 1000000) % 100;
        float z = (float)(longitude * 1000000) % 100;

        vehicle3D.gameObject.transform.position = new Vector3(x, 0, z);

        System.Random random = new System.Random();
        float r = random.Next(0, 8) * 45.0f;
        vehicle3D.gameObject.transform.rotation = Quaternion.Euler(0, r, 0);
    }

}
