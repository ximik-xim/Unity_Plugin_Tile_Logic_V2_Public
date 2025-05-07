using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Логика рандомного выбора префаба из списка префабов
/// </summary>
public class LogicSelectPrefabTileRandom : AbsLogicSelectPrefabTile
{
    public override MarkTilePrefab GetTile(List<MarkTilePrefab> listPrefab)
    {
        var typeId= Random.Range(0, listPrefab.Count);
        return listPrefab[typeId];
    }
}
