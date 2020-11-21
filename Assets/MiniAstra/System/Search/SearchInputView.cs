using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using System;
using UniRx;
using Zenject;

public class SearchInputView : MonoBehaviour
{
    public TextMeshProUGUI searchText;
    private SignalBus _signalBus;

    [Inject]
    private void Inject(SignalBus signalBus)
    {

        _signalBus = signalBus;
    }

    // Start is called before the first frame update
    void Start()
    {
        searchText.ObserveEveryValueChanged(x => x.text)
            .Throttle(TimeSpan.FromMilliseconds(500))
            .Subscribe( x => SendSearchText(x) );
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void SendSearchText(string text)
    {
        Debug.Log("[SendSearchText]");
        _signalBus.Fire(new SearchTextSignal(text));
    }

}


class SearchTextSignal
{
    public SearchTextSignal(string text)
    {
        Text = text;
    }

    public string Text { get; private set; }
}
