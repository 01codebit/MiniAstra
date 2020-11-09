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
    private IDisposable _openTweenDisp = null;
    private Tween _closeTween = null;
    private IDisposable _closeTweenDisp = null;
    private bool _opening = false;
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
        _closeButton.onClick.AddListener(() => Close());

        _signalBus = signalBus;
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
        BindModel(v);
    }

    public void OnDespawned()
    {
        _pool = null;
    }

    public void Dispose()
    {
        _signalBus.Fire(new PanelCloseSignal(this));
        _pool.Despawn(this);
    }
    
    public void Show()
    {
        Debug.Log("[Show()]");
        if (_opening)
            return;

        if (_closeTween != null)
        {
            _closeTween.Kill();
            _closing = false;
        }

        if (_openTween == null)
        {
            transform.localScale = Vector3.zero;
            _openTweenDisp?.Dispose();
            _openTween = transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), _duration);
            _openTweenDisp = _openTween.AsObservable().Subscribe(
                _ => EmptyAction(),
                _ => EmptyAction(),
                () => OnOpenComplete());
            _opening = true;
        }
    }

    void OnOpenComplete()
    {
        _openTween?.Kill();
        _opening = false;
        _openTweenDisp.Dispose();
    }


    public void Close()
    {
        if (_closing)
            return;
        // return Observable.Empty<Unit>();
        
        if (_openTween != null)
        {
            _openTween.Kill();
            _opening = false;
        }

        if (_closeTween == null)
        {
            _closeTween = transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), _duration);
            _closeTweenDisp = _closeTween.AsObservable().Subscribe(
                _ => EmptyAction(),
                _ => EmptyAction(),
                () => OnCloseComplete());

            _closing = true;
        }
    }
    
    void OnCloseComplete()
    {
        _closeTween?.Kill();
        _closing = false;
        _closeTweenDisp.Dispose();
    }

    void EmptyAction()
    {
        
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