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

public class VehiclePanelsCoordinator : IInitializable
{
    private SignalBus _signalBus;

    private VehicleDetailsPanelView.Factory _panelFactory;
//    private Vehicle3D.Factory _urbanoFactory;
//    private Vehicle3D.Factory _extraurbanoFactory;

    //private List<VehicleDetailsPanelView> _panels;

    private ConcurrentDictionary<string, VehicleDetailsPanelView> _panels;
    
    private RectTransform _canvasTransform;

    private TextMeshProUGUI _scoreLabel;

    readonly int MaxPanelsCount = 3;

    [Inject]
    private void Inject(SignalBus signalBus,
        VehicleDetailsPanelView.Factory panelFactory,
        [Inject (Id = "Canvas")] RectTransform canvasTransform,
        [Inject (Id = "ScoreLabelText")] TextMeshProUGUI scoreLabel)
    {
        _signalBus = signalBus;
        _panelFactory = panelFactory;
        _canvasTransform = canvasTransform;
        _scoreLabel = scoreLabel;

//        _urbanoFactory = urbanoFactory;
//        _extraurbanoFactory = extraurbanoFactory;
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

        _panels = new ConcurrentDictionary<string, VehicleDetailsPanelView>();
        _scoreLabel.text = "Panels: 0";

//        _vehicles3D = new Directory<Vehicle3D>();
//        _vehicles3D.Initialize();
    }


    void RemoveOldPanels()
    {
        if (_panels.Count > MaxPanelsCount)
        {
            //Debug.Log("troppi pannelli!");
            var panel = _panels.Values.First();
            DoPanelClose(panel);
        }
    }

    void DoPanelClose(VehicleDetailsPanelView panelToClose)
    {
        string vid = panelToClose.GetVehicleId();
        var disp = panelToClose.Close();
        disp.Subscribe(_ =>
        {
            if (_panels.TryRemove(vid, out var panel))
            {
                panel.Dispose();
                //Debug.Log("dispose pannello _panels.Count: " + _panels.Count);
                _scoreLabel.text = "Panels: " + _panels.Count.ToString();
                RemoveOldPanels();
/*
                Vehicle3D vehicle3d = _vehicles3D.GetItem(vid);
                _vehicles3D.RemoveItem(vid);
                vehicle3d?.Dispose();
*/
            }
        });
    }
    

    public void OnPanelCloseSignal(PanelCloseSignal signal)
    {
        var sender = signal.Panel;
        var vid = sender.GetVehicleId();
        var result = _panels[vid];
        if (result != null)
        {
            //Debug.Log("panels: " + _panels.Count);
            DoPanelClose(result);
        }
    }


    void OpenPanel(PanelOpenSignal signal)
    {
        //Debug.Log("[OpenPanel]");
        var v = signal.SelectedVehicle;

        VehicleDetailsPanelView result = null;
        if(_panels.ContainsKey(v.Id))
            result = _panels[v.Id];

        if (result != null)
        {
            //Debug.Log("panello trovato");

            if (result.Closing())
            {
                //Debug.Log("panello in chiusura, lo riapro");
                result.Show();
            }
            else
            {
                //Debug.Log("panello da chiudere");
                DoPanelClose(result);
            }
        }
        else
        {
            //Debug.Log("panello da creare e aprire");

            //Debug.Log("_panels.Count: " + _panels.Count);

            VehicleDetailsPanelView newPanel = _panelFactory.Create(v);
            newPanel.transform.SetParent(_canvasTransform);
            newPanel.Show();
            
            System.Random random = new System.Random();
            int x = random.Next(-400, 400);
            int y = random.Next(-300, 300);
            newPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);

            if(_panels.TryAdd(v.Id, newPanel))
            {
                //Debug.Log("panel added");
            }
            //Debug.Log("after Add: _panels.Count=" + _panels.Count);

/*
            if(v.Avm)
            {
                Vehicle3D vehicle3D = null;
                if(v.CodiceFamiglia==model.CodiceFamigliaEnum.EXTRAURBANO)
                    vehicle3D = _extraurbanoFactory.Create(v);
                else
                    vehicle3D = _urbanoFactory.Create(v);
                
//                _vehicles3D.AddItem(v.Id, vehicle3D);
            }
*/
        }

        _scoreLabel.text = "Panels: " + _panels.Count.ToString();
        RemoveOldPanels();
    }
}
