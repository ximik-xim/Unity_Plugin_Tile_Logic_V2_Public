using System;
using UnityEngine;

public class ExampleStartLogicTileSplane : MonoBehaviour
{
    [SerializeField] 
    private StartSetStartSplineStorage _startSet;

    [SerializeField] 
    private SetGMPositionSpliteTargetGM _setGmPos;

    private void Awake()
    {
        _startSet.SetSpline();
        
        _setGmPos.StartLogic();
    }
}
