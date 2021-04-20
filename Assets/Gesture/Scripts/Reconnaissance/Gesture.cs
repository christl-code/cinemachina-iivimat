using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Gesture structure
/// </summary>
namespace iivimat
{
[System.Serializable]
public struct Gesture
{
    // Name of the gesture
    public string name;

    // Datas of the fingers (position relative to the base of the hand) 
    public List<Vector3> fingerPositionDatas;

    // Normal of the hand
   // public Vector3 normal;

    // Event to triggered when the gesture is recognize
    public UnityEvent onRecognized;

    // Event to trigerred when the gesture end
    public UnityEvent onEnd;
}    
}

