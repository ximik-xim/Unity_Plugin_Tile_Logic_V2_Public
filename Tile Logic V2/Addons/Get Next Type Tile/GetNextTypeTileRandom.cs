using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Опредление следующего типа таила на основе рандома
/// </summary>
public class GetNextTypeTileRandom : AbsGetNextTypeTile
{
    [SerializeField] 
    private AbsGetDataListTypeTile _typeTile;
    
    public override KeyExampleTipeTile GetTypeTile(KeyExampleTipeTile lastKey)
    {
        var types = _typeTile.GetAllType();
        var typeId= Random.Range(0, types.Count);
        return types[typeId];

    }
}
