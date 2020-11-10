using System;
using DG.Tweening;
using model;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using WhiteEssentials.Extension;
using Zenject;

public class VehicleDetailsPanelView : MonoBehaviour, IPoolable<Vehicle, IMemoryPool>, IDisposable
{
    private TextMeshProUGUI _titleLabel;
    private Image _typeImage;
    private Button _closeButton;

    private Tween _openTween = null;
    private Tween _closeTween = null;
    private bool _closing = false;

    private VehicleSettings _settings;

    private Vehicle _vehicle = null;

    private SignalBus _signalBus;

    readonly float _duration = 2.0f;

    [Inject]
    private void Inject(
        [Inject(Id = "TitleLabel")] TextMeshProUGUI titleLabel,
        [Inject(Id = "TypeImage")] Image typeImage,
        [Inject(Id = "CloseButton")] Button closeButton,
        VehicleSettings settings,
        SignalBus signalBus)
    {
        _titleLabel = titleLabel;
        _typeImage = typeImage;
        _settings = settings;

        _closeButton = closeButton;
        _closeButton.onClick.AddListener(() => SendCloseSignal());

        _signalBus = signalBus;
    }

    void SendCloseSignal()
    {
        _signalBus.Fire(new PanelCloseSignal(this));
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

    public string GetVehicleId()
    {
        return this._vehicle?.Id;
    }

    public class Factory : PlaceholderFactory<Vehicle, VehicleDetailsPanelView>
    {
    }

    IMemoryPool _pool;

    public void OnSpawned(Vehicle v, IMemoryPool pool)
    {
        _pool = pool;
        transform.localScale = Vector3.zero;
        BindModel(v);
    }

    public void OnDespawned()
    {
        _pool = null;
    }

    public void Dispose()
    {
        //_signalBus.Fire(new PanelCloseSignal(this));
        _pool.Despawn(this);
    }

    public bool Closing()
    {
        return _closing;
    }
    
    public void Show()
    {
        Debug.Log("[Show()]");
        if (_closing)
        {
            _closeTween?.Kill();
            _closing = false;
        }

        _openTween = transform.DOScale(Vector3.one, _duration);
    }
    
    public IObservable<Unit> Close()
    {
        //if (_closing)
        //    return Observable.Empty<Unit>();
        
        _openTween?.Kill();

        _closeTween = transform.DOScale(Vector3.zero, _duration);

        _closing = true;
        return _closeTween.AsObservable()
            .Where(x => x==DotweenState.Complete)
            .Select(_ => Unit.Default);
            //.AsUnitObservable();
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