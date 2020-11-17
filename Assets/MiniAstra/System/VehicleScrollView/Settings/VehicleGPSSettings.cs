using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


[CreateAssetMenu(fileName = "VehicleGPSSettings", menuName = "VehicleScrollView/VehicleGPSSettings")]
public class VehicleGPSSettings : SerializedScriptableObject
{
    public Dictionary<bool, Sprite> EnabledGPSToImage = new Dictionary<bool, Sprite>();
}