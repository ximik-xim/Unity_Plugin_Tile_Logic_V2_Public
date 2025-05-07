using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Получение типов с Monobeh
/// </summary>
public class GetDataListTypeTileMono : AbsGetDataListTypeTile
{
    [SerializeField] 
    private DataListTypeTileMono _dataListTypeTileMono;

    public override event Action OnInit
    {
        add
        {
            _dataListTypeTileMono.OnInit += value;
        }
        remove
        {
            _dataListTypeTileMono.OnInit += value;
        }
    }

    public override bool IsInit => _dataListTypeTileMono.IsInit;
    public override List<MarkTilePrefab> GetTiles(KeyExampleTipeTile key)
    {
        return _dataListTypeTileMono.GetTiles(key);
    }

    public override List<KeyExampleTipeTile> GetAllType()
    {
        return _dataListTypeTileMono.GetAllType();
    }
}
