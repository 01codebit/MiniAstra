using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Zenject;

using System;
using model;
using jsonloader;


namespace service
{
    public class VehicleService : IAsstraService
    {
        public string path = @"Assets/StreamingAssets/Json/vehicle_list_full.json";

        private Vehicle[] vehicles = null;

        private JsonLoader<Vehicle> _loader;

        [Inject]
        private void Inject(JsonLoader<Vehicle> loader)
        {
            _loader = loader;
        }

        public IObservable<Vehicle> GetVehicles()
        {
            return Observable.Create<Vehicle> (
                (observer) =>
                {
                    if(vehicles==null)
                        try
                        {
                            if(_loader==null)
                                Debug.Log("[VehicleService.GetVehicles] loader is null");
                            else
                                vehicles = _loader.LoadObjectsJson(this.path);
                        }
                        catch(Exception e)
                        {
                            Debug.Log(e.Message);
                        }

                    foreach(var x in vehicles)
                    {
                        observer.OnNext(x);
                    }

                    observer.OnCompleted();
                    return Disposable.Empty;
                });
        }
    }
}
