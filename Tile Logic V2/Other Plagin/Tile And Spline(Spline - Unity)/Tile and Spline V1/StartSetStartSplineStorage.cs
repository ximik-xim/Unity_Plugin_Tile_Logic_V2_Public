using System;
using UnityEngine;
using UnityEngine.Splines;

public class StartSetStartSplineStorage : MonoBehaviour
{
    [SerializeField] 
    private SplineContainer _setContainer;
    
    [SerializeField] 
    private SplineContainer _getContainer;

    [SerializeField] 
    private SplineOffsetSplineContainer _splineOffset;

    public void SetSpline()
    {
        _splineOffset.AddSpline(_setContainer,_getContainer);
    }
}
