using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Zenject;

using System;
using TMPro;
using model;


public class VehicleButtonView : MonoBehaviour, IPoolable<Vehicle, IMemoryPool>, IDisposable
{
    private TextMeshProUGUI _numberLabel;
    private TextMeshProUGUI _avmLabel;
    private TextMeshProUGUI _statusLabel;
    private TextMeshProUGUI _turnLabel;
    private TextMeshProUGUI _typeLabel;

    private Image _avmImage;
    private Image _typeImage;

    public Sprite defaultIcon;
    public Sprite autocarroIcon;
    public Sprite autovetturaIcon;
    public Sprite extraurbanoIcon;
    public Sprite nccIcon;
    public Sprite scuolabusIcon;
    public Sprite urbanoIcon;

    private Button _button;

    private VehicleSettings _settings;

    private Vehicle _vehicle = null;

    private SignalBus _signalBus;

    [Inject]
    private void Inject([Inject (Id="VehicleButton")] Button button,
        [Inject (Id="NumberLabel")] TextMeshProUGUI numberLabel,
        [Inject (Id="AvmLabel")] TextMeshProUGUI avmLabel,
        [Inject (Id="StatusLabel")] TextMeshProUGUI statusLabel,
        [Inject (Id="TurnLabel")] TextMeshProUGUI turnLabel,
        [Inject (Id="TypeLabel")] TextMeshProUGUI typeLabel,
        [Inject (Id="AvmImage")] Image avmImage,
        [Inject (Id="TypeImage")] Image typeImage,
        VehicleSettings settings,
        SignalBus signalBus)
    {
        _button = button;

        _numberLabel = numberLabel;
        _avmLabel = avmLabel;
        _statusLabel = statusLabel;
        _turnLabel = turnLabel;
        _typeLabel = typeLabel;

        _avmImage = avmImage;
        _typeImage = typeImage;

        _settings = settings;

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

        String result = v.Id.Substring(v.Id.Length - 5);
        this._numberLabel.SetText(result);
        string avm = v.Avm ? "AVM OK" : "NO AVM";
        this._avmLabel.SetText(avm);
        this._statusLabel.SetText("IN ESERCIZIO");
        this._turnLabel.SetText("NO T.MACCH.");
        this._typeLabel.SetText(CodiceFamigliaStr.TypeMap[v.CodiceFamiglia]);
        this._typeImage.sprite = _settings.VehicleTypeToImage[v.CodiceFamiglia];

        //_button.onClick.AddListener(() => OpenPanel());
        _button.OnClickAsObservable().Subscribe(_ => OpenPanel());
    }


    private void OpenPanel()
    {
        _signalBus.Fire(new PanelOpenSignal(this._vehicle));
    }


    public IObservable<Unit> OnPress() => _button.onClick.AsObservable().Select(x => Unit.Default);


    // custom IObservable
    public IObservable<Vehicle> ObservableOnPress()
    {
        return Observable.Create<Vehicle> (
            (observer) =>
            {
                if(this._vehicle!=null)
                {
                    _button.onClick.AddListener(() => observer.OnNext(this._vehicle));
                }
                else
                {
                    //Debug.LogError("no vehicle configured");
                }
                //button.onClick.AddListener(() => observer.OnCompleted());
                return Disposable.Empty;
            });
    }


    public class Factory : PlaceholderFactory<Vehicle, VehicleButtonView>
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
