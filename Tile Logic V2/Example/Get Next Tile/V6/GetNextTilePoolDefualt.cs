using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetNextTilePoolDefualt : AbsGetNextTile
{
    public override event Action OnInit;
    public override bool IsInit => true;
    
    [SerializeField] 
    private PoolTileDefault _poolData;
    
    public override MarkTilePrefab GetNextTile(DKOKeyAndTargetAction lastTileDKO = null)
    {
        var prefab = _poolData.GetTilePrefab();
        
        return prefab;
    }
}
