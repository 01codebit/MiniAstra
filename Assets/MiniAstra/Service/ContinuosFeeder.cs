using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;
using System;
using UniRx;

using model;

using WhiteEssentials.Extension;

namespace service
{
    public class ContinuosFeeder : IInitializable, IObserver<Vehicle>, IAsstraService
    {

        private VehicleService _service;
        private List<Vehicle> _vehicles;


        [Inject]
        public void Inject(VehicleService service)
        {
            _service = service;
        }

        public void Initialize()
        {
            _vehicles = new List<Vehicle>();

            //_service.GetVehicles().Subscribe(x => AddVehicle(x), x => LoadingCompleted());
            _service.GetVehicles().Subscribe(this);
        }

        public void OnNext(Vehicle v)
        {
            Debug.Log("[OnNext]");
            AddVehicle(v);
        }

        public void OnError(Exception error)
        {
            Debug.Log("[OnError] error: " + error.ToString());
        }

        public void OnCompleted()
        {
            Debug.Log("[OnCompleted]");
        }


        public void AddVehicle(Vehicle x)
        {
            Debug.Log("[AddVehicle]");
            _vehicles.Add(x);
        }

        public void LoadingCompleted()
        {
            Debug.Log("[LoadingCompleted]");
        }


        private void UpdateCoords()
        {
            foreach(var x in _vehicles)
            {
                if(x.Location!=null && x.Location.Coordinates!=null)
                {
                    var lat = x.Location.Coordinates[0];
                    var lon = x.Location.Coordinates[1];

                    System.Random random = new System.Random();
                    float r1 = random.Next(-1, 1);
                    float r2 = random.Next(-1, 1);

                    float flat = (float)lat + r1;
                    float flon = (float)lon + r2;

                    x.Location.Coordinates[0] = flat;
                    x.Location.Coordinates[1] = flon;
                }
            }
        }


        public IObservable<Vehicle> GetVehicles()
        {
/*
            return Observable.Create<Vehicle> (
                (observer) =>
                {
                    while(true) {
                        foreach(var x in _vehicles)
                        {
                            UpdateCoords(x);
                            observer.OnNext(x);

                            Thread.Sleep(1000);
                        }                        
                    }
                });
*/
            return Observable
                .Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5))
                .Do(x => UpdateCoords())
                .SelectMany(_vehicles.ToObservable());
        }
    }
}