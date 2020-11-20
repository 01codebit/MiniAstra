using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

using System;
using model;


namespace service
{
    public interface IAsstraService
    {
        IObservable<Vehicle> GetVehicles();
    }
}
