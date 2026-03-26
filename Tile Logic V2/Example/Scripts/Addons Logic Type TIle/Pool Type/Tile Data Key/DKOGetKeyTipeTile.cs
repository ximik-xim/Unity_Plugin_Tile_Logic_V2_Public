using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Обертка через DKO для инкопсуляции действий
/// (по идеи получить значение типа (ключа) сможет, тот кто имеет ключ для получени класса)
/// </summary>
public class DKOGetKeyTipeTile  : DKOTargetAction
{
    [SerializeField] 
    private KeyDataExampleTipeTileMono _data;

    private DKODataGetKeyTipeTile _dataGet;

    private void Awake()
    {
        _dataGet = new DKODataGetKeyTipeTile(_data);
        LocalAwake();
    }

    protected override DKODataRund InvokeRun()
    {
        if (_dataGet == null)
        {
            _dataGet = new DKODataGetKeyTipeTile(_data);
        }

        return _dataGet;
    }

}


public class DKODataGetKeyTipeTile : DKODataRund
{
    public DKODataGetKeyTipeTile(KeyDataExampleTipeTileMono data)
    {
        _data = data;
    }
    
    private KeyDataExampleTipeTileMono _data;

    public KeyExampleTipeTile GetKey()
    {
        return _data.GetKey();
    }
}