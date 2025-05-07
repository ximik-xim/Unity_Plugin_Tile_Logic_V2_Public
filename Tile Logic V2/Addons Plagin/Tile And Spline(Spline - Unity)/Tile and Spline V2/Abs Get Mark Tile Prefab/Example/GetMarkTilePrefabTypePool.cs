using System;
using UnityEngine;

public class GetMarkTilePrefabTypePool : AbsGetMarkTilePrefab
{
    [SerializeField] 
    private GetDataSO_KeyExampleTipeTile _keyTipeTile;

    [SerializeField] 
    private PoolTileType _poolTile;

    public override event Action OnInit
    {
        add
        {
            _poolTile.OnInit += value;
        }

        remove
        {
            _poolTile.OnInit -= value;
        }
    }

    public override bool IsInit => _poolTile.IsInit;

    public override MarkTilePrefab GetTilePrefab()
    {
        return _poolTile.GetTileExample(_keyTipeTile.GetData());
    }
}
