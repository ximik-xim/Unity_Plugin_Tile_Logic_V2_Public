using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Служит для получения напровления движения таилов
/// </summary>
public abstract class AbsGetForwardRunTile : MonoBehaviour
{
    public abstract bool IsInit { get; }
    public abstract event Action OnInit;

    public abstract Vector3 GetForward();
}
