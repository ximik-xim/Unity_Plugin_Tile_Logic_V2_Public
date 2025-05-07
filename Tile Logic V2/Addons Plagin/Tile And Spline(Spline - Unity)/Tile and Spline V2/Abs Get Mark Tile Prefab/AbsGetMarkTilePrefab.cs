using System;
using UnityEngine;

public abstract class AbsGetMarkTilePrefab : MonoBehaviour
{
    public abstract event Action OnInit;
    public abstract bool IsInit { get; }
    
    public abstract MarkTilePrefab GetTilePrefab();
}
