using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// пример возращение таила по запросу
/// (всего с 1 префабом и без буфера, для максимального упрощения)
/// </summary>
public class GetNextTileOnePrefab : AbsGetNextTile
{
    public override event Action OnInit;
    public override bool IsInit => true;

    [SerializeField] 
    private MarkTilePrefab _tilePrefab;
    
    public override MarkTilePrefab GetNextTile(DKOKeyAndTargetAction lastTileDKO = null)
    {
        var prefab = Instantiate(_tilePrefab);
        
        return prefab;
    }
}
