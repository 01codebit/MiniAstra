using System.Collections;
using System.Collections.Generic;
using Zenject;
using UnityEngine;


public class VehiclePanelsCoordinator : IInitializable
{
    private SignalBus _signalBus;

    private VehicleDetailsPanelView.Factory _panelFactory;

    private List<VehicleDetailsPanelView> _panels;

    private RectTransform _canvasTransform;

    [Inject]
    private void Inject(SignalBus signalBus,
        VehicleDetailsPanelView.Factory panelFactory,
        [Inject (Id = "Canvas")] RectTransform canvasTransform)
    {
        _signalBus = signalBus;
        _panelFactory = panelFactory;
        _canvasTransform = canvasTransform;
    }


    public void Initialize()
    {
        _signalBus.Subscribe<PanelOpenSignal>(OpenPanel);
        _signalBus.Subscribe<PanelCloseSignal>(OnPanelCloseSignal);
        _panels = new List<VehicleDetailsPanelView>();
    }

    public void OnPanelCloseSignal(PanelCloseSignal signal)
    {
        var sender = signal.Panel;

        foreach(var x in _panels)
        {
            if(x==sender)
            {
                _panels.Remove(x);
                break;
            }
        }
    }


    void OpenPanel(PanelOpenSignal signal)
    {

        var v = signal.SelectedVehicle;

        VehicleDetailsPanelView newPanel = _panelFactory.Create(v);
        newPanel.transform.SetParent(_canvasTransform);

        System.Random _random = new System.Random();
        int x = _random.Next(-400, 400);
        int y = _random.Next(-300, 300);
        newPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);

        _panels.Add(newPanel);
        if(_panels.Count>3)
            _panels[0].Close();
    }

}
