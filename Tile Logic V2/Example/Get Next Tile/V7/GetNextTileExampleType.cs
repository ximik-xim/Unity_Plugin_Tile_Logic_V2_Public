using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Логика запроса следующего таила, на основе типа последне обноруженного таила(его типа)
/// (тип таила получаю через DKO таила)
/// </summary>
public class GetNextTileExampleType : AbsGetNextTile
{
    public override event Action OnInit
    {
        add
        {
            _logicSpawnNextTile.OnInit += value;
        }

        remove
        {
            _logicSpawnNextTile.OnInit -= value;
        }
    }
    public override bool IsInit => _logicSpawnNextTile.IsInit;
    
    [SerializeField]
    private AbsGetNextTypeTile _getNextTypeTile;

    [SerializeField] 
    private ListAbsGetExampleTileTypeTile _logicSpawnNextTile;
    
    [SerializeField] 
    private GetDataSODataDKODataKey _keyGetTypeTile;


    public override MarkTilePrefab GetNextTile(DKOKeyAndTargetAction lastTileDKO = null)
    {
        //Если по какой то причине не будет получаться получать черезе DKO тип у таилов,
        //то значит нужно будет сделать отдельный скрипт в который засовывать тип таила который
        //был затригерен последним  

        var data = (DKODataGetKeyTipeTile)lastTileDKO.KeyRun(_keyGetTypeTile.GetData());

        var nextTipeTile = _getNextTypeTile.GetTypeTile(data.GetKey());

        var exampleNextTile = _logicSpawnNextTile.GetNextTile(nextTipeTile);

        return exampleNextTile;
    }
}
