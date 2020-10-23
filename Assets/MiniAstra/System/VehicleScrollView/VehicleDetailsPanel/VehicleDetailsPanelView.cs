using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Zenject;
using DG.Tweening;

using System;
using TMPro;
using model;


public class VehicleDetailsPanelView : MonoBehaviour, IPoolable<Vehicle, IMemoryPool>, IDisposable
{
    private TextMeshProUGUI _titleLabel;
    private Image _typeImage;
    private Button _closeButton;

    private VehicleSettings _settings;

    private Vehicle _vehicle = null;

    private SignalBus _signalBus;

    [Inject]
    private void Inject(
        [Inject (Id="TitleLabel")] TextMeshProUGUI titleLabel,
        [Inject (Id="TypeImage")] Image typeImage,
        [Inject (Id="CloseButton")] Button closeButton,
        VehicleSettings settings,
        SignalBus signalBus)
    {
        _titleLabel = titleLabel;
        _typeImage = typeImage;
        _settings = settings;

        _closeButton = closeButton;
        _closeButton.onClick.AddListener(() => Close());

        _signalBus = signalBus;
    }

    public Button CloseButton()
    {
        return this._closeButton;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void BindModel(Vehicle v)
    {
        this._vehicle = v;

        String title = v.Id.Substring(v.Id.Length - 5);
        this._titleLabel.SetText(title);

        this._typeImage.sprite = _settings.VehicleTypeToImage[v.CodiceFamiglia];
    }


    public class Factory : PlaceholderFactory<Vehicle, VehicleDetailsPanelView>
    {
    }

    IMemoryPool _pool;

    public void OnSpawned(Vehicle v, IMemoryPool pool)
    {
        _pool = pool;
        BindModel(v);

        Show();
    }

    public void OnDespawned()
    {
        _pool = null;
    }

    public void Dispose()
    {
        _pool.Despawn(this);
    }

    public void Show()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 0.2f);
    }

    public void Close()
    {
        //var sequence = new DOTween.Sequence();

        _signalBus.Fire(new PanelCloseSignal(this));
        transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), 0.2f).OnComplete(() => Dispose());
    }
}



public class PanelOpenSignal
{
    public PanelOpenSignal(Vehicle vehicle)
    {
        SelectedVehicle = vehicle;
    }

    public Vehicle SelectedVehicle { get; private set; }

}


public class PanelCloseSignal
{
    public PanelCloseSignal(VehicleDetailsPanelView panel)
    {
        Panel = panel;
    }

    public VehicleDetailsPanelView Panel { get; private set; }
}

