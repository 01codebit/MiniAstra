using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Zenject;

using System;
using TMPro;
using model;


public class VehicleButtonView : MonoBehaviour
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

    [Inject]
    private void Inject([Inject (Id="VehicleButton")] Button button,
        [Inject (Id="NumberLabel")] TextMeshProUGUI numberLabel,
        [Inject (Id="AvmLabel")] TextMeshProUGUI avmLabel,
        [Inject (Id="StatusLabel")] TextMeshProUGUI statusLabel,
        [Inject (Id="TurnLabel")] TextMeshProUGUI turnLabel,
        [Inject (Id="TypeLabel")] TextMeshProUGUI typeLabel,
        [Inject (Id="AvmImage")] Image avmImage,
        [Inject (Id="TypeImage")] Image typeImage)
    {
        _button = button;

        _numberLabel = numberLabel;
        _avmLabel = avmLabel;
        _statusLabel = statusLabel;
        _turnLabel = turnLabel;
        _typeLabel = typeLabel;

        _avmImage = avmImage;
        _typeImage = typeImage;
    }


    private Vehicle vehicle = null;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {        
    }

    public void BindModel(Vehicle v)
    {
        this.vehicle = v;

        String result = v.Id.Substring(v.Id.Length - 5);
        this._numberLabel.SetText(result);
        string avm = v.Avm ? "AVM OK" : "NO AVM";
        this._avmLabel.SetText(avm);
        this._statusLabel.SetText("IN ESERCIZIO");
        this._turnLabel.SetText("NO T.MACCH.");

        this._typeLabel.SetText(CodiceFamigliaStr.TypeMap[v.CodiceFamiglia]);

        Sprite sp = defaultIcon;
        switch(v.CodiceFamiglia)
        {
            case CodiceFamigliaEnum.AUTOCARRO:
                sp = autocarroIcon;
                break;
            case CodiceFamigliaEnum.AUTOVETTURA:
                sp = autovetturaIcon;
                break;
            case CodiceFamigliaEnum.EXTRAURBANO:
                sp = extraurbanoIcon;
                break;
            case CodiceFamigliaEnum.NOLEGGIO_CON_CONDUCENTE:
                sp = nccIcon;
                break;
            case CodiceFamigliaEnum.SCUOLABUS:
                sp = scuolabusIcon;
                break;
            case CodiceFamigliaEnum.URBANO:
                sp = urbanoIcon;
                break;
            default:
                sp = defaultIcon;
                break;
        }
        this._typeImage.sprite = sp;
    }


    public IObservable<Unit> OnPress() => _button.onClick.AsObservable().Select(x => Unit.Default);


    // custom IObservable
    public IObservable<Vehicle> ObservableOnPress()
    {
        return Observable.Create<Vehicle> (
            (observer) =>
            {
                if(this.vehicle!=null)
                    _button.onClick.AddListener(() => observer.OnNext(this.vehicle));
                else
                {
                    //Debug.LogError("no vehicle configured");
                }
                //button.onClick.AddListener(() => observer.OnCompleted());
                return Disposable.Empty;
            });
    }


    public class Factory : PlaceholderFactory<VehicleButtonView>
    {
    }
}
