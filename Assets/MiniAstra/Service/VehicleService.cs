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
    public class VehicleService
    {
        public string path;

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
                        vehicles = _loader.LoadObjectsJson(this.path);

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
