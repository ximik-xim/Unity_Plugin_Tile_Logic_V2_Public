using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DKOGetInfoPositionConnectNextTile : DKOTargetAction
{
    [SerializeField] 
    private PositionConnectNextTile _positionConnectNextTileInfo;

    private DKODataGetInfoPositionConnectNextTile _data;
    
    private void Awake()
    {
        _data = new DKODataGetInfoPositionConnectNextTile(_positionConnectNextTileInfo);
        LocalAwake();
    }

    protected override DKODataRund InvokeRun()
    {
        if (_data == null)
        {
            _data = new DKODataGetInfoPositionConnectNextTile(_positionConnectNextTileInfo);
        }
        
        return _data;
    }
    
}

public class DKODataGetInfoPositionConnectNextTile : DKODataRund
{
    public DKODataGetInfoPositionConnectNextTile(PositionConnectNextTile positionConnectNextTileInfo)
    {
        _positionConnectNextTileInfo = positionConnectNextTileInfo;
    }
    
    private PositionConnectNextTile _positionConnectNextTileInfo;

    public PositionConnectNextTile PositionConnectNextTileInfo => _positionConnectNextTileInfo;
}


