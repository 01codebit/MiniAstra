using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;
using System;

using model;


public class Vehicle3D : MonoBehaviour, IPoolable<Vehicle, IMemoryPool>, IDisposable
{

    private Vehicle _vehicle;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BindModel(Vehicle v)
    {
        //Debug.Log("[Vehicle3D.BindModel] ...");
        _vehicle = v;
    }

    public string GetDestination()
    {
        return _vehicle.DestinationName;
    }

    public class Factory : PlaceholderFactory<Vehicle, Vehicle3D>
    {
    }


    IMemoryPool _pool;

    public void OnSpawned(Vehicle v, IMemoryPool pool)
    {
        _pool = pool;
        BindModel(v);
    }

    public void OnDespawned()
    {
        _pool = null;
    }

    public void Dispose()
    {
        _pool.Despawn(this);
    }
}
