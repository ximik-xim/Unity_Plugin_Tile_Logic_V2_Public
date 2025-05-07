using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SetPositionTileData
{
    public SetPositionTileData(DKOKeyAndTargetAction tileDKO, AbsTilePositionInfo tilePositionInfo)
    {
        _tileDKO = tileDKO;
        _tilePositionInfo = tilePositionInfo;
    }

    private DKOKeyAndTargetAction _tileDKO;
    public DKOKeyAndTargetAction TileDKO => _tileDKO;
    
    private AbsTilePositionInfo _tilePositionInfo;
    public AbsTilePositionInfo TilePositionInfo => _tilePositionInfo;
}
