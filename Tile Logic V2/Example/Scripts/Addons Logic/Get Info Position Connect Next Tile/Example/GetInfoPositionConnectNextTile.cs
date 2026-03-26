using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetInfoPositionConnectNextTile : AbsGetInfoPositionConnectNextTile
{
    /// <summary>
    /// за одно, может служить стартовой точкой спавна
    /// </summary>
    [SerializeField]
    private Vector3 _positionConnectNextTile;

    public override void SetPositionConnectNextTile(Vector3 positionConnectNextTile)
    {
        _positionConnectNextTile = positionConnectNextTile;
    }

    public override Vector3 GetPositionConnectNextTile()
    {
        return _positionConnectNextTile;
    }
    
}
