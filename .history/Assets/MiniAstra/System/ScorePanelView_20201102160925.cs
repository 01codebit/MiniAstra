using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

using UniRx;
using TMPro;

public class ScorePanelView : MonoBehaviour
{

    public TextMeshPro label;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetObservable().Subscribe(x => Reaction(x));
    }


    public UniRx.IObservable<int> GetObservable()
    {
        return Observable.Create<int>(
            (observer) =>
            {
                for(int i = 0; i<10; i++)
                {
                    observer.OnNext(i*3);
                    Thread.Sleep(1000);
                }
                return Disposable.Empty;
            }
        );
    
    }

    public void Reaction(int x)
    {
        label.text = "Panels: " + x;
    }
}
