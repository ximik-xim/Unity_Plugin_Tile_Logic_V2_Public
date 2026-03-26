using System;
using UnityEngine;
using UnityEngine.Splines;

public abstract class AbsGetNextSpline : MonoBehaviour
{
    public abstract bool IsInit { get; }
    public abstract event Action OnInit;
    
    public abstract Spline GetNextSpline();
}
