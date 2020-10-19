using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

using System;
using TMPro;
using model;


public class VehicleButtonView : MonoBehaviour
{

    public TextMeshProUGUI numberLabel;
    public TextMeshProUGUI avmLabel;
    public TextMeshProUGUI statusLabel;
    public TextMeshProUGUI turnLabel;
    public TextMeshProUGUI typeLabel;

    public Image avmImage;
    public Image typeImage;

    public Sprite defaultIcon;
    public Sprite autocarroIcon;
    public Sprite autovetturaIcon;
    public Sprite extraurbanoIcon;
    public Sprite nccIcon;
    public Sprite scuolabusIcon;
    public Sprite urbanoIcon;

    public Button button;

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
        this.numberLabel.SetText(result);
        string avm = v.Avm ? "AVM OK" : "NO AVM";
        this.avmLabel.SetText(avm);
        this.statusLabel.SetText("IN ESERCIZIO");
        this.turnLabel.SetText("NO T.MACCH.");

        this.typeLabel.SetText(CodiceFamigliaStr.TypeMap[v.CodiceFamiglia]);

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
        this.typeImage.sprite = sp;
    }


    public IObservable<Unit> OnPress() => button.onClick.AsObservable().Select(x => Unit.Default);


    // custom IObservable
    public IObservable<Vehicle> ObservableOnPress()
    {
        return Observable.Create<Vehicle> (
            (observer) =>
            {
                if(this.vehicle!=null)
                    button.onClick.AddListener(() => observer.OnNext(this.vehicle));
                else
                {
                    //Debug.LogError("no vehicle configured");
                }
                //button.onClick.AddListener(() => observer.OnCompleted());
                return Disposable.Empty;
            });
    }

}
