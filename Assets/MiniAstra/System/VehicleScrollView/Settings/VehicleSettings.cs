using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

using model;


[CreateAssetMenu(fileName = "VehicleButtonSettings", menuName = "VehicleScrollView/VehicleSettings")]
public class VehicleSettings : SerializedScriptableObject
{
    public Dictionary<CodiceFamigliaEnum, Sprite> VehicleTypeToImage = new Dictionary<CodiceFamigliaEnum, Sprite>();
}