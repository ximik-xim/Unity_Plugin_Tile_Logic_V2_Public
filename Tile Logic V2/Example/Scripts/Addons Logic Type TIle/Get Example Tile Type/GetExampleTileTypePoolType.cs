using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Делаю запрос на полечение экземпляра таила через пул
/// </summary>
public class GetExampleTileTypePoolType : AbsGetExampleTileType
{
    [SerializeField] 
    private PoolTileType _poolTypeTile;

    public override MarkTilePrefab GetTileExample(KeyExampleTipeTile type)
    {
        return _poolTypeTile.GetTileExample(type);
    }
}
