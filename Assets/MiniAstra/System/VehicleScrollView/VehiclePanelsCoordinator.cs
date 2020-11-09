using System.Collections;
using System.Collections.Generic;
using Zenject;
using UnityEngine;

using System;
using System.Collections.Specialized;
using System.Linq;
using UniRx;


public class VehiclePanelsCoordinator : IInitializable
{
    private SignalBus _signalBus;

    private VehicleDetailsPanelView.Factory _panelFactory;

    //private List<VehicleDetailsPanelView> _panels;

    private SortedDictionary<string, VehicleDetailsPanelView> _panels;
    
    
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
        //_signalBus.Subscribe<PanelOpenSignal>(OpenPanel);
        //_signalBus.Subscribe<PanelCloseSignal>(OnPanelCloseSignal);

        _signalBus.GetStream<PanelOpenSignal>()
            .SubscribeOn(Scheduler.ThreadPool)
            .ObserveOnMainThread()
            .Subscribe((s) => OpenPanel(s));

        _signalBus.GetStream<PanelCloseSignal>()
            .SubscribeOn(Scheduler.ThreadPool)
            .ObserveOnMainThread()
            .Subscribe((s) => OnPanelCloseSignal(s));

        _panels = new SortedDictionary<string, VehicleDetailsPanelView>();
    }


    public void OnPanelCloseSignal(PanelCloseSignal signal)
    {
        var sender = signal.Panel;
        var vid = sender.GetVehicleId();
        var result = _panels[vid];
        if (result != null)
        {
            _panels.Remove(vid);
            sender.Close();
            Debug.Log("panels: " + _panels.Count);
        }
    }


    void OpenPanel(PanelOpenSignal signal)
    {
        var v = signal.SelectedVehicle;

        VehicleDetailsPanelView result = null;
        if(_panels.ContainsKey(v.Id))
            result = _panels[v.Id];
        if (result != null)
        {
            _panels.Remove(v.Id);
            Debug.Log("panels: " + _panels.Count);
            result.Close();
        }
        else
        {
            VehicleDetailsPanelView newPanel = _panelFactory.Create(v);
            newPanel.transform.SetParent(_canvasTransform);
            newPanel.Show();
            
            System.Random random = new System.Random();
            int x = random.Next(-400, 400);
            int y = random.Next(-300, 300);
            newPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);

            _panels.Add(v.Id, newPanel);
            Debug.Log("panels: " + _panels.Count);
            if (_panels.Count > 3)
            {
                var panel = _panels.Values.First();
                _panels.Remove(panel.GetVehicleId());
                panel.Close();
            }
        }
    }

}
