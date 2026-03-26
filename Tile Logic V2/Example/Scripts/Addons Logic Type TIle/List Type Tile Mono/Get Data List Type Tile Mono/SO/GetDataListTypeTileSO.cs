using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Получение типов с SO
/// </summary>
public class GetDataListTypeTileSO : AbsGetDataListTypeTile
{
    [SerializeField] 
    private DataListTypeTileSO _dataSO;

    public override event Action OnInit
    {
        add
        {
            _dataSO.OnInit += value;
        }
        remove
        {
            _dataSO.OnInit -= value;
        }
    }

    public override bool IsInit => _dataSO.IsInit;

    public override List<MarkTilePrefab> GetTiles(KeyExampleTipeTile key)
    {
        return _dataSO.GetTiles(key);
    }

    public override List<KeyExampleTipeTile> GetAllType()
    {
        return _dataSO.GetAllType();
    }
}
