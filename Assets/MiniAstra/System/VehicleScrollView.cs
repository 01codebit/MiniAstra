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
    public Transform contentPanel;
    // A: public SimpleObjectPool objectPool;


    public GameObject prefab;

    private VehicleService _service;

    [Inject]
    private void Inject(VehicleService service)
    {
        _service = service;
    }

    // Start is called before the first frame update
    void Start()
    {
        string filepath = @"Assets/StreamingAssets/Json/vehicle_list_full.json";
        _service.path = filepath;
        _service.GetVehicles().Subscribe(x => OnGetVehicle(x));
    }

    void OnGetVehicle(Vehicle v)
    {
        Debug.Log(v.ToString());

    /* A:
        GameObject newButton = objectPool.GetObject();
        newButton.transform.SetParent(contentPanel);

        VehicleButtonView vehicleButton = newButton.GetComponent<VehicleButtonView>();
        vehicleButton.BindModel(v);
    */


        GameObject newButton = (GameObject)GameObject.Instantiate(prefab);
        newButton.transform.SetParent(contentPanel);

        VehicleButtonView vehicleButton = newButton.GetComponent<VehicleButtonView>();
        vehicleButton?.BindModel(v);
    }
}
