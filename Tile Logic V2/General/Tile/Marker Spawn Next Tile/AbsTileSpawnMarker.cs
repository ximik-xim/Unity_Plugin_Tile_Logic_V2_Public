using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// будет находиться на таиле и определять какие возможные действия можно будет сделать над таилом через DKO при его создании(включении)
/// </summary>
public abstract class AbsTileSpawnMarker : MonoBehaviour
{
    [SerializeField] 
    protected DKOKeyAndTargetAction _keyAndTargetAction;

    public DKOKeyAndTargetAction GetTileDKO()
    {
        return _keyAndTargetAction;
    }
}
