using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// будет находиться на таиле и определять какие возможные действия можно будет сделать над таилом через DKO при его смерти(выключении)
/// </summary>
public class AbsTileDeadMarker : MonoBehaviour
{
    [SerializeField] 
    protected DKOKeyAndTargetAction _keyAndTargetAction;

    public DKOKeyAndTargetAction GetTileDKO()
    {
        return _keyAndTargetAction;
    }
}
